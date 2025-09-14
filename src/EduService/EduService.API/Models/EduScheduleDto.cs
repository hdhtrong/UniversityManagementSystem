namespace EduService.API.Models
{
    public class EduScheduleDto
    {
        public Guid ScheduleID { get; set; }

        public Guid? SectionID { get; set; }

        public int DayOfWeek { get; set; }

        public int StartPeriod { get; set; }

        public int EndPeriod { get; set; }

        public string Code { get; set; } = string.Empty;

        public Guid? RoomID { get; set; }

        public string? RoomName { get; set; }
    }
}