namespace EduService.API.Models
{
    public class EduProgramDto
    {
        public Guid ProgramID { get; set; }
        public string ProgramName { get; set; } = string.Empty;
        public string ProgramCode { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public Guid MajorID { get; set; }
    }

}
