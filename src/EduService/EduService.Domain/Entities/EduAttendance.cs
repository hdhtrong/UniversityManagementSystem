using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduService.Domain.Entities
{
    [Table("Attendances")]
    public class EduAttendance : AuditableEntity
    {
        [Key]
        public Guid AttendanceID { get; set; }

        public Guid? EnrollmentID { get; set; }
        
        [ForeignKey("EnrollmentID")]
        public EduEnrollment? Enrollment { get; set; }

        public Guid ScheduleID { get; set; }
        public Guid WeekID { get; set; }

        [ForeignKey(nameof(ScheduleID) + "," + nameof(WeekID))]
        public EduScheduleWeek ScheduleWeek { get; set; } = null!;

        [Required]
        public DateTime AttendanceDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = null!; // Present, Absent, Late, Excused

        [MaxLength(255)]
        public string? Notes { get; set; }
    }
}
