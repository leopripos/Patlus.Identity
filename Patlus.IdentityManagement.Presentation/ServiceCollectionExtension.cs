using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Infrastructure.Cache;
using Patlus.IdentityManagement.Infrastructure.Dispatcher;
using Patlus.IdentityManagement.IntegrationDispatcher;
using Patlus.IdentityManagement.Persistence;
using Patlus.IdentityManagement.Presentation.Services;
using Patlus.IdentityManagement.UseCase;
using Patlus.IdentityManagement.UseCase.Services;

namespace Patlus.IdentityManagement.Presentation
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPresentationCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUseCaseFeatures(configuration);
            services.AddIntegrationDispatcher(configuration);

            services.AddCacheService();
            services.AddTokenCacheStorage();

            return services;
        }

        public static IServiceCollection AddUseCaseFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddMediatR(UseCaseModule.GetBundles());
            services.AddValidatorsFromAssemblies(UseCaseModule.GetBundles());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IPasswordService, HMACSHA1PasswordService>();
            services.AddSingleton<ITokenService, JwtBearerTokenService>();
            services.AddSingleton<IIdentifierService, IdentifierGenerator>();
            services.AddSingleton<ITimeService, TimeService>();

            return services;
        }

        public static IServiceCollection AddIntegrationDispatcher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(IntegrationDispatcherModule.GetBundles());
            services.AddKafkaDispatcher(configuration);

            return services;
        }

    }
}
