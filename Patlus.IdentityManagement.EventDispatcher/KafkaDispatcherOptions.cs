using Confluent.Kafka;
using System.Text.Json;

namespace Patlus.IdentityManagement.EventDispatcher
{
    public class KafkaDispatcherOptions
    {
        public ProducerConfig ProducerConfig = new ProducerConfig();
        public JsonSerializerOptions JsonOptions = new JsonSerializerOptions();
    }
}
