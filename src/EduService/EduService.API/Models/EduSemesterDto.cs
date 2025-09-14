namespace EduService.API.Models
{
    public class EduSemesterDto
    {
        public Guid SemesterID { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public string SemesterCode { get; set; } = string.Empty;
        public int? SemesterOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Activated { get; set; }
        public Guid? YearID { get; set; }
        public EduAcademicYearDto AcademicYear { get; set; }
        public List<EduWeekDto> Weeks { get; set; }
    }

}
