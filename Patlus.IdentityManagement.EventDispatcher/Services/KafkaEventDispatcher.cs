using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Patlus.IdentityManagement.EventDispatcher.Services
{
    public class KafkaEventDispatcher : IEventDispatcher
    {
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IProducer<Guid?, string> _producer;
        private readonly ILogger<KafkaEventDispatcher> _logger;

        public KafkaEventDispatcher(ILogger<KafkaEventDispatcher> logger, IOptions<KafkaDispatcherOptions> options)
        {
            _jsonOptions = options.Value.JsonOptions;
            _producer = new ProducerBuilder<Guid?, string>(options.Value.ProducerConfig).Build();
            _logger = logger;
        }

        public async Task Dispatch<TNotification>(string topic, TNotification notification, Guid? orderGroup = null)
        {
            var messageValue = new
            {
                type = $"{topic}/{typeof(TNotification).Name}",
                body = notification
            };

            var message = new Message<Guid?, string>()
            {
                Key = orderGroup,
                Value = JsonSerializer.Serialize(messageValue, _jsonOptions)
            };

            try
            {
                var result = await _producer.ProduceAsync(topic, message);

                _logger.LogInformation($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                var failure = new
                {
                    e.Error.Reason,
                    Topic = topic,
                    Message = new {
                        key = message.Key,
                        value = message.Value
                    }
               };

                _logger.LogError(e, $"Delivery failed: {failure}");
            }
        }
    }
}
