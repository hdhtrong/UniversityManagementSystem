using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("ExamSessions")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduExamSession : AuditableEntity
    {
        [Key]
        public Guid ExamSessionID { get; set; }

        public Guid SemesterID { get; set; }

        [ForeignKey("SemesterID")]
        public EduSemester Semester { get; set; }

        [MaxLength(150)]
        public string? Name { get; set; } // e.g. "Midterm Exam 2025-2026-01"

        [MaxLength(100)]
        public string? Type { get; set; } // Midterm/Final

        [MaxLength(100)]
        public string? Code { get; set; } 

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<EduExam> Exams { get; set; } = new List<EduExam>();
    }
}
