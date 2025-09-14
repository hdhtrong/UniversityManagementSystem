using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("Instructors")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduInstructor : AuditableEntity
    {
        [Key]
        public Guid ID { get; set; }

        [MaxLength(20)]
        public required string Code { get; set; }

        [MaxLength(150)]
        public required string Fullname { get; set; }

        [MaxLength(20)]
        public required string Gender { get; set; }

        [MaxLength(20)]
        public string? Type { get; set; } // Giảng viên

        [MaxLength(10)]
        public string? DOB { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(50)]
        public string? Degree { get; set; }

        [MaxLength(50)]
        public string? Nationality { get; set; }

        [MaxLength(50)]
        public string? PersonalPhone { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        public Guid? DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public EduDepartment? Department { get; set; }

        public ICollection<EduClass>? Classes { get; set; }

        public ICollection<EduCourseSection>? CourseSections { get; set; }
    }
}
