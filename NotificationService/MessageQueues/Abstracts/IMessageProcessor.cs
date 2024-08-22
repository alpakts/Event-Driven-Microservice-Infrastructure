namespace NotificationService.MessageQueues.Abstracts;
public interface IMessageProcessor<T>
{
    Task ProcessMessageAsync(T message);
}
