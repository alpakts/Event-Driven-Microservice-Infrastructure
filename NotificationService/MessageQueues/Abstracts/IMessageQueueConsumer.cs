namespace NotificationService.MessageQueues.Abstracts
{
    public interface IMessageQueueConsumer<T>
    {
        Task ConsumeAsync(string topicOrQueue);
    }

}
