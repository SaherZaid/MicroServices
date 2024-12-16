using BookingService.API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using BookingService.API.Dtos;
using BookingService.API.Models;
using BookingService.API.RabbitMQ;
using BookingService.API.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:4940");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<RabbitMqService>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

builder.Services
    .AddCors(options =>
        options.AddPolicy(
            "Booking",
            policy =>
                policy
                    .WithOrigins("https://localhost:7151", "https://localhost:7173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
        )
    );


var app = builder.Build();

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("Booking");


// 🟢 POST endpoint to create a booking and publish it (without repository or unit of work)
app.MapPost("/bookings", async (CreateBookingDto bookingDto, RabbitMqService rabbitMqService) =>
{
    try
    {
        // Directly handle the booking creation logic here
        var newBooking = new Booking
        {
            RoomId = bookingDto.RoomIdDto,
            CustomerId = bookingDto.CustomerIdDto,
            CheckInDate = bookingDto.CheckInDateDto,
            CheckOutDate = bookingDto.CheckOutDateDto
        };

        // Optionally, you can simulate storing the booking (since you requested no repo/unitOfWork)
        // For example, save the booking to a database or any storage.

        // Serialize the created booking to JSON
        var createdBooking = new BookingDto
        {
            BookingIdDto = newBooking.Id,  // Assume the ID will be generated upon saving
            RoomIdDto = bookingDto.RoomIdDto,
            CustomerIdDto = bookingDto.CustomerIdDto,
            CheckInDateDto = bookingDto.CheckInDateDto,
            CheckOutDateDto = bookingDto.CheckOutDateDto
        };

        // Publish the booking details to RabbitMQ
        var jsonBooking = JsonSerializer.Serialize(createdBooking);
        rabbitMqService.PublishMessage(jsonBooking);

        // Return CreatedAtAction with the booking details
        return Results.Created($"/bookings/{createdBooking.BookingIdDto}", createdBooking);
    }
    catch (Exception ex)
    {
        // Handle any exceptions by returning a problem with a message
        return Results.Problem("An error occurred while creating the booking.", statusCode: 500);
    }
});


app.Run();
