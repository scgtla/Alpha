using System;
using Alpha.Models.Room;
using Alpha.Models.User;

namespace Alpha.Models.Booking
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public bool Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public UserModel User { get; set; }
        public RoomModel Room { get; set; }
    }
}