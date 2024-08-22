namespace NotificationService.MessageQueues.Abstracts
{
    public interface IMessageQueueConsumer<T>
    {
        void Consume(string topicOrQueue);
    }

}
