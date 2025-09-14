using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Domain.Entities
{
    [Table("Weeks")]
    [Index(nameof(WeekCode), IsUnique = true)]
    public class EduWeek : AuditableEntity
    {
        [Key]
        public Guid WeekID { get; set; }

        public required int WeekNumber { get; set; } // 1

        [MaxLength(50)]
        public required string WeekName { get; set; } // Tuần 1 - Học kỳ 1 - Năm học 2025-2026

        [MaxLength(20)]
        public required string WeekCode { get; set; } // 2025-2026-01-01

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Guid? SemesterID { get; set; }

        [ForeignKey("SemesterID")]
        public EduSemester? Semester { get; set; }
        public ICollection<EduScheduleWeek>? ScheduleWeeks { get; set; }
    }
}
