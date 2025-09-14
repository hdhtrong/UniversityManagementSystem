namespace EduService.API.Models
{
    public class EduClassDto
    {
        public Guid ClassID { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public Guid? ProgramID { get; set; }
        public Guid? InstructorID { get; set; }
    }

}
