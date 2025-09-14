namespace EduService.API.Models
{
    public class EduInstructorDto
    {
        public Guid ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Fullname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? DOB { get; set; }
        public string? Title { get; set; }
        public string? Degree { get; set; }
        public string? Nationality { get; set; }
        public string? PersonalPhone { get; set; }
        public string? Email { get; set; }
        public Guid? DepartmentID { get; set; }
    }

}
