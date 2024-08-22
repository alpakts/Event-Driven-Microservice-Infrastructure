namespace NotificationService.MessageQueues.Abstracts
{
    public interface IMessageQueueProducer<T>
    {
        Task ProduceAsync(string topicOrQueue, T message);
    }

}
