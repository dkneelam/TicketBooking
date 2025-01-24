namespace TicketBookingAPI.Models
{
    public class Booking
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string EventName { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
