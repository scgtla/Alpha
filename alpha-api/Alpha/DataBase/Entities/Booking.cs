using System;

#nullable disable

namespace Alpha.DataBase.Entities
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public bool Status { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
