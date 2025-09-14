namespace EduService.API.Models
{
    public class EduAcademicYearDto
    {
        public Guid YearID { get; set; }
        public string YearName { get; set; } = string.Empty;
        public string YearCode { get; set; } = string.Empty;
        public int? Order { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Activated { get; set; }
        public List<EduSemesterDto> Semesters { get; set; }
    }

}
