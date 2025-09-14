using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("AcademicYears")]
    [Index(nameof(YearName), IsUnique = true)]
    [Index(nameof(YearCode), IsUnique = true)]
    public class EduAcademicYear : AuditableEntity
    {
        [Key]
        public Guid YearID { get; set; }

        [MaxLength(50)]
        public required string YearName { get; set; } // Năm học 2025-2026

        [MaxLength(20)]
        public required string YearCode { get; set; } // 2025-2026

        public int? Order { get; set; } // 1

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool? Activated { get; set; }

        public ICollection<EduSemester>? Semesters { get; set; }
    }
}
