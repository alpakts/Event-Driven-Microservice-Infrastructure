using Confluent.Kafka;
using Newtonsoft.Json;
using NotificationService.MessageQueues.Abstracts;
namespace NotificationService.MessageQueues.Concretes.Kafka;
public class KafkaConsumer<T> : IMessageQueueConsumer<T>
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly IMessageProcessor<T> _messageProcessor;

    public KafkaConsumer(string brokerList, string groupId, IMessageProcessor<T> messageProcessor)
    {
        var config = new ConsumerConfig
        {
            GroupId = groupId,
            BootstrapServers = brokerList,
            AutoOffsetReset = AutoOffsetReset.Earliest,
             EnableAutoCommit = true
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _messageProcessor = messageProcessor;
    }

    public async Task ConsumeAsync(string topic)
    {
        _consumer.Subscribe(topic);
        while (true)
        {
            try
            {
                var consumeResult = _consumer.Consume(CancellationToken.None);
                var message = JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
                await _messageProcessor.ProcessMessageAsync(message);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
