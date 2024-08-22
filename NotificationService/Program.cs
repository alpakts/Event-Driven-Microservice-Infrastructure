using NotificationService.MessageQueues.Abstracts;
using NotificationService.MessageQueues.Concretes.Kafka;
using NotificationService.MessageQueues.Concretes.Kafka.Processors;
using NotificationService.Models;
using NotificationService.Services;

var emailService = new EmailService(
               smtpServer: "mock.com",
               smtpPort: 587,
               smtpUser: "mock@mock.com",
               smtpPass: "mock"
           );

var messageProcessor = new UserRegisteredMessageProcessor(emailService);
IMessageQueueConsumer<UserRegisteredEvent> kafkaConsumer = new KafkaConsumer<UserRegisteredEvent>("localhost:9092", "notification-service", messageProcessor);

Task.Run(() =>
{
    kafkaConsumer.Consume("user-registered");
});
Console.WriteLine("Notification Service is running...");
Console.ReadLine();