using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduService.Domain.Entities
{
    public class EduGrade : AuditableEntity
    {
        [Key]
        public Guid GradeID { get; set; }

        public Guid? EnrollmentID { get; set; }

        [ForeignKey("EnrollmentID")]
        public EduEnrollment? Enrollment { get; set; }

        public double? AssignmentScore { get; set; }
        public double? MidtermScore { get; set; }
        public double? FinalExamScore { get; set; }

        public double? Total100Score { get; set; }
        public double? Total10Score { get; set; }
        public double? Total4Score { get; set; }

        [MaxLength(5)]
        public string? LetterGrade { get; set; }

        public bool? Passed { get; set; }
    }
}
