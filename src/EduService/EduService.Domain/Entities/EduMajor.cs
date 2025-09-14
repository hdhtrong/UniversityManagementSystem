using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("Majors")]
    [Index(nameof(MajorCode), IsUnique = true)]
    public class EduMajor : AuditableEntity
    {
        [Key]
        public Guid MajorID { get; set; }

        [MaxLength(200)]
        public required string MajorName { get; set; }

        [MaxLength(50)]
        public required string MajorCode { get; set; }

        [MaxLength(50)]
        public required string MajorType { get; set; } // Trong nước, Liên kết

        public int? Order { get; set; }

        public Guid? DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public required EduDepartment? Department { get; set; }

        public ICollection<EduProgram>? Programs { get; set; }
    }
}
