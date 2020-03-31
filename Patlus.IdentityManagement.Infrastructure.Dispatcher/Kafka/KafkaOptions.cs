using Confluent.Kafka;
using System.Text.Json;

namespace Patlus.IdentityManagement.Infrastructure.Dispatcher.Kafka
{
    public class KafkaOptions
    {
        public ProducerConfig ProducerConfig = new ProducerConfig();
        public JsonSerializerOptions JsonOptions = new JsonSerializerOptions();
    }
}
