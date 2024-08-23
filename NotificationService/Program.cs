using Microsoft.Extensions.Configuration;
using NotificationService.MessageQueues.Abstracts;
using NotificationService.MessageQueues.Concretes.Kafka;
using NotificationService.MessageQueues.Concretes.Kafka.Processors;
using NotificationService.Models;
using NotificationService.Services;
using NotificationService.Settings;
var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // Projenin çalıştığı dizini alır
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // appsettings.json dosyasını ekler
               .Build();

var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();

var emailService = new EmailService(emailSettings);

var messageProcessor = new UserRegisteredMessageProcessor(emailService);
IMessageQueueConsumer<UserRegisteredEvent> kafkaConsumer = new KafkaConsumer<UserRegisteredEvent>("localhost:9092", "notification-service", messageProcessor);

Task.Run(() =>
{
    kafkaConsumer.ConsumeAsync("user-registered");
});
Console.WriteLine("Notification Service is running...");
Console.ReadLine();