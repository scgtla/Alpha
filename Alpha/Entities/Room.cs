using System;
using System.Collections.Generic;

#nullable disable

namespace Alpha.Entities
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int RoomId { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public bool HasProjector { get; set; }
        public bool HasWhiteboard { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
