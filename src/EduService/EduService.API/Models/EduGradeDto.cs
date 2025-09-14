namespace EduService.API.Models
{
    public class EduGradeDto
    {
        public Guid GradeID { get; set; }
        public Guid EnrollmentID { get; set; }

        // Thông tin Enrollment phụ
        public string StudentName { get; set; } = string.Empty;
        public string SectionCode { get; set; } = string.Empty;

        public double? AssignmentScore { get; set; }
        public double? MidtermScore { get; set; }
        public double? FinalExamScore { get; set; }

        public double? Total100Score { get; set; }
        public double? Total10Score { get; set; }
        public double? Total4Score { get; set; }

        public string? LetterGrade { get; set; }
        public bool? Passed { get; set; }
    }
}
