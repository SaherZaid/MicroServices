namespace BookingService.API.Models;

public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; } // the room booked
    public int CustomerId { get; set; } // the customer who booked
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}