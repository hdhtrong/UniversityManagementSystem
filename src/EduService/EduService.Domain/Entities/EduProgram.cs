using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduService.Domain.Entities
{
    [Table("Programs")]
    [Index(nameof(ProgramCode), IsUnique = true)]
    public class EduProgram : AuditableEntity
    {
        [Key]
        public Guid ProgramID { get; set; }

        [MaxLength(100)]
        public required string ProgramName { get; set; }

        [MaxLength(50)]
        public required string ProgramCode { get; set; }

        public int? StartYear { get; set; }

        public int? EndYear { get; set; }

        public required Guid MajorID { get; set; }

        [ForeignKey("MajorID")]
        public required EduMajor Major { get; set; }

        public ICollection<EduClass>? Classes { get; set; }

        public ICollection<EduCurriculum>? Curriculums { get; set; }
    }

}
