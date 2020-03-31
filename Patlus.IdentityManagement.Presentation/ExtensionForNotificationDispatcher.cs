using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.IdentityManagement.Infrastructure.Dispatcher;

namespace Patlus.IdentityManagement.Presentation
{
    public static class ExtensionForNotificationDispatcher
    {
        public static IServiceCollection AddNotificationDispatcher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(NotificationDispatcherModule.GetBundles());
            services.AddKafkaDispatcher(configuration);

            return services;
        }

        public static IMapperConfigurationExpression AddNotificationDispatcherMappings(this IMapperConfigurationExpression config)
        {
            config.AddMaps(NotificationDispatcherModule.GetBundles());

            return config;
        }
    }
}
