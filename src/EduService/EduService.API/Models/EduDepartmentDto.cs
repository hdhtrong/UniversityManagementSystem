namespace EduService.API.Models
{
    public class EduDepartmentDto
    {
        public Guid DepartmentID { get; set; }
        public int Order { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string? EnglishName { get; set; }
        public string? Category { get; set; }
    }
}
