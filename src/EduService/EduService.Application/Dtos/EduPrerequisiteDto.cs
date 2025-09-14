namespace EduService.Application.Dtos
{
    public class EduPrerequisiteDto
    {
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int? Order { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
    }
}
