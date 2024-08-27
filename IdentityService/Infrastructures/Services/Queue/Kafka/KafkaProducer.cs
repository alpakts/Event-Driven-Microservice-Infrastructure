using Confluent.Kafka;
using Newtonsoft.Json;

namespace IdentityService.Infrastructures.Services.Queue.Kafka
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(string brokerList)
        {
            var config = new ProducerConfig { BootstrapServers = brokerList };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync<T>(string topic, T message)
        {
            var jsonMessage = JsonConvert.SerializeObject(message);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
        }
    }
}
