using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Patlus.Common.Presentation.Json;
using Patlus.Common.Presentation.Services;
using Patlus.IdentityManagement.EventDispatcher.Services;

namespace Patlus.IdentityManagement.EventDispatcher
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddKafkaDispatcher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<KafkaDispatcherOptions>().Configure(options =>
            {
                var kafkaConfig = configuration.GetSection("EventDispatcher:Kafka");
                options.ProducerConfig.BootstrapServers = kafkaConfig[nameof(ProducerConfig.BootstrapServers)];
                options.ProducerConfig.SaslMechanism = SaslMechanism.Plain;
                options.ProducerConfig.SaslUsername = kafkaConfig[nameof(ProducerConfig.SaslUsername)];
                options.ProducerConfig.SaslPassword = kafkaConfig[nameof(ProducerConfig.SaslPassword)];

                options.JsonOptions.PropertyNamingPolicy = JsonNamingPolicies.PascalCase;
                options.JsonOptions.DictionaryKeyPolicy = JsonNamingPolicies.PascalCase;
            });
            services.AddSingleton<IEventDispatcher, KafkaEventDispatcher>();

            return services;
        }
    }
}
