using System;
using Alpha.Models.Room;
using Alpha.Models.User;

namespace Alpha.Models.Booking
{
    public class BookingShortInfo
    {
        public bool Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}