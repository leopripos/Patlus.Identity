using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Patlus.Common.Presentation.Responses;
using Patlus.Common.Presentation.Services;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.Infrastructure.Dispatcher.Kafka
{
    public class KafkaDispatcher : IEventDispatcher
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaDispatcher> _logger;

        public KafkaDispatcher(ILogger<KafkaDispatcher> logger, IOptions<KafkaOptions> options)
        {
            _jsonOptions = options.Value.JsonOptions;
            _producer = new ProducerBuilder<string, string>(options.Value.ProducerConfig).Build();
            _logger = logger;
        }

        public async Task DispatchAsync<TNotification>(string topic, TNotification notification, Guid? orderGroup, CancellationToken cancellationToken)
            where TNotification : IDto
        {
            var messageValue = new
            {
                Type = $"{topic}/{typeof(TNotification).Name}",
                Body = notification
            };

            var message = new Message<string, string>()
            {
                Key = orderGroup.HasValue ? orderGroup.ToString() : null,
                Value = JsonSerializer.Serialize(messageValue, _jsonOptions)
            };

            try
            {
                var result = await _producer.ProduceAsync(topic, message, cancellationToken);

                _logger.LogInformation($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                var failure = new
                {
                    e.Error.Reason,
                    Topic = topic,
                    Message = new
                    {
                        key = message.Key,
                        value = message.Value
                    }
                };

                _logger.LogError(e, $"Delivery failed: {failure}");
            }
        }
    }
}
