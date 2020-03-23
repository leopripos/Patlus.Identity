using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.UseCase.Behaviours;
using Patlus.Common.UseCase.Services;
using Patlus.IdentityManagement.Cache;
using Patlus.IdentityManagement.EventDispatcher;
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
            services.AddDatabase(configuration);
            services.AddUseCaseFeatures();
            services.AddMachineService();

            services.AddCacheService();
            services.AddTokenCacheService();

            services.AddNotificationDispatcher(configuration);

            return services;
        }

        public static IServiceCollection AddUseCaseFeatures(this IServiceCollection services)
        {
            services.AddMediatR(UseCaseModule.GetBundles());
            services.AddValidatorsFromAssemblies(UseCaseModule.GetBundles());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddSingleton<IPasswordService, HMACSHA1PasswordService>();
            services.AddSingleton<ITokenService, JwtBearerTokenService>();

            return services;
        }

        public static IServiceCollection AddNotificationDispatcher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(NotificationDispatcherModule.GetBundles());
            services.AddAutoMapper(NotificationDispatcherModule.GetBundles());
            services.AddKafkaDispatcher(configuration);

            return services;
        }

        public static IServiceCollection AddMachineService(this IServiceCollection services)
        {
            services.AddSingleton<ITimeService, TimeService>();
            return services;
        }
    }
}
