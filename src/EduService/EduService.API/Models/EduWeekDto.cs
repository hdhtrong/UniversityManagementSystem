namespace EduService.API.Models
{
    public class EduWeekDto
    {
        public Guid WeekID { get; set; }
        public int WeekNumber { get; set; }
        public string WeekName { get; set; } = default!;
        public string WeekCode { get; set; } = default!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? SemesterID { get; set; }
    }

}
