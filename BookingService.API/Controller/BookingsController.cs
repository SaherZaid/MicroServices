//using BookingService.API.Dtos;
//using BookingService.API.Models;
//using BookingService.API.RabbitMQ;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.API.Controller;

//[ApiController]
//[Route("api/[controller]")]
//public class BookingsController : ControllerBase
//{
//    private readonly IUnitOfWork _unitOfWork;
//    private readonly RabbitMqService _rabbitMqService;

//    public BookingsController(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//        _rabbitMqService = rabbitMqService;
//    }

//    // GET: api/Bookings
//    [HttpGet]
//    public async Task<IActionResult> GetAllBookings()
//    {
//        var bookings = await _unitOfWork.BookingRepository.GetAllAsync();
//        var bookingDtos = bookings.Select(b => new BookingDto
//        {
//            BookingIdDto = b.Id,
//            RoomIdDto = b.RoomId,
//            CustomerIdDto = b.CustomerId,
//            CheckInDateDto = b.CheckInDate,
//            CheckOutDateDto = b.CheckOutDate
//        }).ToList();

//        return Ok(bookingDtos);
//    }


//    // POST: api/Bookings
//    [HttpPost]
//    public async Task<IActionResult> AddBooking([FromBody] CreateBookingDto bookingDto)
//    {
//        // Check room availability
//        var isAvailable = !await _unitOfWork.BookingRepository
//            .FindAsync(b =>
//                b.RoomId == bookingDto.RoomIdDto &&
//                ((bookingDto.CheckInDateDto >= b.CheckInDate && bookingDto.CheckInDateDto < b.CheckOutDate) ||
//                 (bookingDto.CheckOutDateDto > b.CheckInDate && bookingDto.CheckOutDateDto <= b.CheckOutDate) ||
//                 (bookingDto.CheckInDateDto <= b.CheckInDate && bookingDto.CheckOutDateDto >= b.CheckOutDate)));

//        if (!isAvailable)
//        {
//            return BadRequest("Room is not available for the selected dates.");
//        }

//        // Convert CreateBookingDto to Booking entity
//        var newBooking = new Booking
//        {
//            RoomId = bookingDto.RoomIdDto,
//            CustomerId = bookingDto.CustomerIdDto,
//            CheckInDate = bookingDto.CheckInDateDto,
//            CheckOutDate = bookingDto.CheckOutDateDto
//        };

//        // Add the booking to the database
//        await _unitOfWork.BookingRepository.AddAsync(newBooking);
//        await _unitOfWork.CompleteAsync();

//        // Create a BookingMessage
//        var bookingMessage = new BookingMessage
//        {
//            RoomId = newBooking.RoomId,
//            CustomerId = newBooking.CustomerId,
//            CheckInDate = newBooking.CheckInDate,
//            CheckOutDate = newBooking.CheckOutDate
//        };

//        // Publish the message to RabbitMQ using RabbitMqService
//        _rabbitMqService.SendBookingMessage(bookingMessage);

//        // Prepare response DTO
//        var createdBooking = new BookingDto
//        {
//            BookingIdDto = newBooking.Id,
//            RoomIdDto = bookingDto.RoomIdDto,
//            CustomerIdDto = bookingDto.CustomerIdDto,
//            CheckInDateDto = bookingDto.CheckInDateDto,
//            CheckOutDateDto = bookingDto.CheckOutDateDto
//        };

//        // Return the response
//        return Ok(createdBooking);
//    }

//}


