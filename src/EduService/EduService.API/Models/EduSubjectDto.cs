namespace EduService.API.Models
{
    public class EduSubjectDto
    {
        public Guid SubjectID { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public int Credits { get; set; }
        public int TheoryHours { get; set; }
        public int PracticeHours { get; set; }
    }
}
