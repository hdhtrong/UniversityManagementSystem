using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("Enrollments")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduEnrollment : AuditableEntity
    {
        [Key]
        public Guid EnrollmentID { get; set; }

        public Guid? StudentID { get; set; }

        [ForeignKey("StudentID")]
        public EduStudent? Student { get; set; }

        public Guid? SectionID { get; set; }

        [ForeignKey("SectionID")]
        public EduCourseSection? Section { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public ICollection<EduGrade>? Grades { get; set; }

        public ICollection<EduAttendance>? Attendances { get; set; }

        [MaxLength(100)]
        public string? Code { get; set; } // SectionCode-StudentID

        [MaxLength(100)]
        public string? Status { get; set; } // Cancelled, Dropped, Studying, Ended..v.v
    }
}
