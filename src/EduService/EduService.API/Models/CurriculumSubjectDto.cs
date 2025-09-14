namespace EduService.API.Models
{
    public class CurriculumSubjectDto
    {
        public Guid SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramCode { get; set; }
        public int Credits { get; set; }
        public int SemesterOrder { get; set; }
        public bool Compulsory { get; set; }
    }
}
