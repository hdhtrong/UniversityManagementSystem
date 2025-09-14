using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace EduService.Domain.Entities
{
    [Table("Schedules")]
    public class EduSchedule : AuditableEntity
    {
        [Key]
        public Guid ScheduleID { get; set; }

        public Guid? SectionID { get; set; }

        [ForeignKey("SectionID")]
        public EduCourseSection? Section { get; set; }

        public int DayOfWeek { get; set; }  // 2 = Monday ... 7 = Sunday

        public int StartPeriod { get; set; }

        public int EndPeriod { get; set; }

        [MaxLength(100)]
        public required string Code { get; set; } // CourseSectionCode-DayOfWeek

        public Guid? RoomID { get; set; }

        [ForeignKey("RoomID")]
        public EduRoom? Room { get; set; }

        public ICollection<EduScheduleWeek>? ScheduleWeeks { get; set; }
    }
}
