namespace BookingService.API.Dtos;

public class BookingDto
{
    public int BookingIdDto { get; set; }
    public int RoomIdDto { get; set; }
    public int CustomerIdDto { get; set; }
    public DateTime CheckInDateDto { get; set; }
    public DateTime CheckOutDateDto { get; set; }
}