
namespace EduService.API.Models
{
    public class EduPeriodDto
    {
        public int PeriodNumber { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}
