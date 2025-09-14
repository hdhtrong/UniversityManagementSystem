
namespace EduService.Application.Dtos
{
    public class EduWeekScheduleDto
    {
        public int WeekNumber { get; set; }
        public string WeekName { get; set; }
        public List<EduScheduleItemDto> Items { get; set; } = new();
    }
}
