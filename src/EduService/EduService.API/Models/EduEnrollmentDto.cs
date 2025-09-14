namespace EduService.API.Models
{
    public class EduEnrollmentDto
    {
        public Guid EnrollmentID { get; set; }

        public Guid? StudentID { get; set; }
        public string? StudentName { get; set; } // lấy từ Student.FullName (nếu có)

        public Guid? SectionID { get; set; }
        public string? SectionCode { get; set; } // lấy từ Section.Code

        public DateTime? EnrollmentDate { get; set; }

        public string? Code { get; set; } // SectionCode-StudentID
        public string? Status { get; set; } // Cancelled, Dropped, Studying, Ended...
    }
}
