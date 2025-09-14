using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("SubjectPrerequisites")]
    [PrimaryKey(nameof(SubjectID), nameof(PrerequisiteSubjectID))]
    public class EduSubjectPrerequisite : AuditableEntity
    {
        public Guid SubjectID { get; set; }

        public Guid PrerequisiteSubjectID { get; set; }

        public int? Order { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        [MaxLength(250)]
        public string? Note { get; set; }

        [ForeignKey(nameof(SubjectID))]
        public EduSubject? Subject { get; set; }

        [ForeignKey(nameof(PrerequisiteSubjectID))]
        public EduSubject? PrerequisiteSubject { get; set; }
    }
}
