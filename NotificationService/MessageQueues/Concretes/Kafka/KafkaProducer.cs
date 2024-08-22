using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.MessageQueues.Abstracts;
namespace NotificationService.MessageQueues.Concretes.Kafka;
public class KafkaProducer<T> : IMessageQueueProducer<T>
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(string brokerList)
    {
        var config = new ProducerConfig { BootstrapServers = brokerList };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, T message)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
    }
}
