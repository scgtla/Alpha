using System;
using Alpha.Models.Room;
using Alpha.Models.User;

namespace Alpha.Models.Booking
{
    public class BookingCreate
    {
        public int BookingId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}