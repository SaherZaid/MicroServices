namespace BookingService.API.Models;

public class Sender
{
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public string QueueName { get; set; }
}