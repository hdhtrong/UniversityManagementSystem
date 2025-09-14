namespace EduService.API.Models
{
    public class EduRoomDto
    {
        public Guid RoomID { get; set; }
        public string? Building { get; set; }
        public int Floor { get; set; }
        public string? RoomName { get; set; }
        public int Capacity { get; set; }
    }
}
