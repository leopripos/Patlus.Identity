using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Persistence.Contexts;
using Patlus.IdentityManagement.Rest.Authentication;
using Patlus.IdentityManagement.Rest.Filters.Exceptions;
using Patlus.IdentityManagement.Rest.Policies;
using Patlus.IdentityManagement.Rest.Services;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patlus.IdentityManagement.Rest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opt => {
                    opt.Filters.Add(new NotFoundExceptionFilter());
                    opt.Filters.Add(new ValidationExceptionFilter());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddMediatR(ModuleProfile.GetBundles());
            services.AddValidatorsFromAssemblies(ModuleProfile.GetBundles());
            services.AddAutoMapper(GetType().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<ITimeService>(new TimeService());
            services.AddSingleton<ITokenService>(new JwtTokenService(Configuration));
            services.AddSingleton<IPasswordService>(new HMACSHA1PasswordService(Configuration));

            services.AddHttpContextAccessor();
            services.AddScoped<IPoolResolver, PoolResolver>();

            services.AddDbContext<IMasterDbContext, MasterDbContext>(opt => {
                opt.UseSqlServer(Configuration["Database:Connection"], x => x.MigrationsAssembly("Patlus.IdentityManagement.Persistence"));
            });

            services.AddScoped<IUserResolver, UserResolver>();
            services.AddTransient(typeof(JwtBearerAuthenticationEvent));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:Jwt:Key"])),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["Authentication:Jwt:Issuer"],
                        ValidAudience = Configuration["Authentication:Jwt:Audience"],
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };

                    opt.EventsType = typeof(JwtBearerAuthenticationEvent);
                });

            services.AddAuthorization(opt =>
            {
                AddAuhtorizationPolicy(opt);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service", Version = "v1" });

                c.CustomSchemaIds(type =>
                {
                    var namespaceParts = type.FullName.Split(".");
                    var featureName = namespaceParts[^2];
                    var dtoName = namespaceParts[^1];

                    return $"{featureName}.{dtoName}";
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                { 
                    Name = "Authorization",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        { 
                            Reference = new OpenApiReference()
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
                        new List<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service V1");
            });
        }

        private void AddAuhtorizationPolicy(AuthorizationOptions opt)
        {
            opt.AddPolicy(PoolPolicy.Read, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(PoolPolicy.Create, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(PoolPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(AccountPolicy.Read, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(AccountPolicy.Create, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(AccountPolicy.UpdateActiveStatus, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.AccessToken);
            });

            opt.AddPolicy(TokenPolicy.Refresh, policy =>
            {
                policy.RequireClaim(ClaimType.AccessType, TokenType.RefreshToken);
            });
        }
    }
}
