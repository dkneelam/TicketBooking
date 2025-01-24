using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TicketBookingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMongoCollection<Booking> _bookingsCollection;

        // MongoDB connection URL and database details
        private const string ConnectionString = "mongodb+srv://root:123@cluster0.wxz8f.mongodb.net/";
        private const string DatabaseName = "Ticketbooking";
        private const string CollectionName = "tickets";

        public BookingsController()
        {
            var client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _bookingsCollection = database.GetCollection<Booking>(CollectionName);
        }

        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
        {
            await _bookingsCollection.InsertOneAsync(booking);
            return Ok(new { message = "Booking data saved successfully.", status = "Success" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingsCollection.Find(_ => true).ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _bookingsCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _bookingsCollection.DeleteOneAsync(b => b.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return Ok(new { message = "Booking deleted successfully.", status = "Success" });
        }
    }
}
