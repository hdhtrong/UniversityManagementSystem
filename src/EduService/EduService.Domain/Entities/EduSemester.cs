using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("Semesters")]
    [Index(nameof(SemesterName), IsUnique = true)]
    [Index(nameof(SemesterCode), IsUnique = true)]
    public class EduSemester : AuditableEntity
    {
        [Key]
        public Guid SemesterID { get; set; }

        [MaxLength(50)]
        public required   string SemesterName { get; set; } // Học kỳ 1-Năm học-2025-2026

        [MaxLength(20)]
        public required string SemesterCode { get; set; } // 2025-2026-01

        public int? SemesterOrder { get; set; } // 1

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Activated { get; set; }

        public Guid? YearID { get; set; }

        [ForeignKey("YearID")]
        public EduAcademicYear? AcademicYear { get; set; }

        public ICollection<EduWeek>? Weeks { get; set; }

        public ICollection<EduExamSession>? ExamSessions { get; set; }
    }

}
