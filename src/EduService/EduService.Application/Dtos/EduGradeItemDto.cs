

namespace EduService.Application.Dtos
{
    public class EduGradeItemDto
    {
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public string SectionCode { get; set; } = null!;
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
