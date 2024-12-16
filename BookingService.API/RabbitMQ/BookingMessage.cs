namespace BookingService.API.RabbitMQ;


public class BookingMessage
{
    public int RoomId { get; set; }
    public int CustomerId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}