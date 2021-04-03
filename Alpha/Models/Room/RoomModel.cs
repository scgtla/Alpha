namespace Alpha.Models.Room
{
    public class RoomModel
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public bool HasProjector { get; set; }
        public bool HasWhiteboard { get; set; }
    }
}