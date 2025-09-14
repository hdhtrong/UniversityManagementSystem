using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Domain.Entities
{
    [Table("Departments")]
    [Index(nameof(DepartmentCode), IsUnique = true)]
    [Index(nameof(ShortName), IsUnique = true)]
    public class EduDepartment : AuditableEntity
    {
        [Key]
        public Guid DepartmentID { get; set; }

        public int Order { get; set; }

        [MaxLength(20)]
        public required string DepartmentCode { get; set; }

        [MaxLength(200)]
        public required string DepartmentName { get; set; }

        [MaxLength(100)]
        public string? ShortName { get; set; }

        [MaxLength(200)]
        public string? EnglishName { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; } // Khoa, phòng ban, Trung tâm

        public ICollection<EduMajor>? Majors { get; set; }
    }
}
