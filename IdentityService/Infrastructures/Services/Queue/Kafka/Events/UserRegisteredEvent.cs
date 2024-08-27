namespace IdentityService.Infrastructures.Services.Queue.Kafka.Events
{
    public class UserRegisteredEvent
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
