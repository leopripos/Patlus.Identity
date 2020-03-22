using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Presentation.Services;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.Presentation
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUseCaseFeatures(this IServiceCollection services)
        {
            services.AddMediatR(ModuleProfile.GetBundles());
            services.AddValidatorsFromAssemblies(ModuleProfile.GetBundles());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IPasswordService, HMACSHA1PasswordService>();
            services.AddSingleton<ITokenService, JwtBearerTokenService>();

            return services;
        }

        public static IServiceCollection AddPassswordHasher(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordService, HMACSHA1PasswordService>();

            return services;
        }
        public static IServiceCollection AddMachineService(this IServiceCollection services)
        {
            services.AddSingleton<ITimeService, TimeService>();
            return services;
        }
    }
}
