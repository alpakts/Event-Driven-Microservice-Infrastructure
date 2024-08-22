using NotificationService.MessageQueues.Abstracts;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.MessageQueues.Concretes.Kafka.Processors;
public class UserRegisteredMessageProcessor : IMessageProcessor<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;

    public UserRegisteredMessageProcessor(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task ProcessMessageAsync(UserRegisteredEvent message)
    {
        var subject = "Welcome to Our Service";
        var body = $"Hello {message.Username},\n\nWelcome to our service! We're glad to have you on board.\n\nBest Regards,\nYour Company";
        await _emailService.SendEmailAsync(message.Email, subject, body);
    }
}
