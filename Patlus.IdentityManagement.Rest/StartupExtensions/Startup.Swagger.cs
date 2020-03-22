using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Patlus.IdentityManagement.Rest.Extensions
{
    public static class StartupSwagger
    {
        public static void ConfigureSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service", Version = "v1" });

                c.CustomSchemaIds(type =>
                {
                    if (type.FullName is null) throw new Exception($"{nameof(type.FullName)} is null");

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

                var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{typeof(StartupSwagger).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(xmlFile);
            });
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service V1");
            });
        }
    }
}
