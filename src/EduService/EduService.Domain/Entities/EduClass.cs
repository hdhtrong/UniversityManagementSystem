using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("AdministrativeClasses")]
    [Index(nameof(ClassCode), IsUnique = true)]
    public class EduClass : AuditableEntity
    {
        [Key]
        public Guid ClassID { get; set; }

        [MaxLength(20)]
        public required string ClassCode { get; set; }

        [MaxLength(250)]
        public required string ClassName { get; set; }

        public int? StartYear { get; set; }

        public Guid? ProgramID { get; set; }

        [ForeignKey("ProgramID")]
        public EduProgram? Program { get; set; }

        public Guid? InstructorID { get; set; }

        [ForeignKey("InstructorID")]
        public EduInstructor? Instructor { get; set; }

        public ICollection<EduStudent>? Students { get; set; }
    }
}
