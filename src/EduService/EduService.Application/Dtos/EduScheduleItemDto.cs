
namespace EduService.Application.Dtos
{
    public class EduScheduleItemDto
    {
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string SectionCode { get; set; } = null!;
        public string InstructorName { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public int DayOfWeek { get; set; }
        public int StartPeriod { get; set; }
        public int EndPeriod { get; set; }
    }
}
