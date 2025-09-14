namespace EduService.API.Models
{
    public class EduMajorDto
    {
        public Guid MajorID { get; set; }
        public string MajorName { get; set; } = string.Empty;
        public string MajorCode { get; set; } = string.Empty;
        public string MajorType { get; set; } = string.Empty;
        public int? Order { get; set; }
        public Guid? DepartmentID { get; set; }
    }
}
