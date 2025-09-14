using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("ScheduleWeeks")]
    [PrimaryKey(nameof(ScheduleID), nameof(WeekID))]
    public class EduScheduleWeek : AuditableEntity
    {
        public Guid ScheduleID { get; set; }

        [ForeignKey("ScheduleID")]
        public EduSchedule? Schedule { get; set; }

        public Guid WeekID { get; set; }

        [ForeignKey("WeekID")]
        public EduWeek? Week { get; set; }

        [MaxLength(250)]
        public string? Note { get; set; }

        [MaxLength(100)]
        public required string Code { get; set; } // CourseSectionCode-DayOfWeek-WeekNumber

        public ICollection<EduAttendance>? Attendances { get; set; }
    }
}
