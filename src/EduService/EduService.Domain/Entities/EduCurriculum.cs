using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Domain.Entities
{
    [Table("Curriculums")]
    [PrimaryKey(nameof(ProgramID), nameof(SubjectID))]
    public class EduCurriculum : AuditableEntity
    {
        public Guid ProgramID { get; set; }

        public Guid SubjectID { get; set; }

        public int SemesterOrder { get; set; }

        public bool Compulsory { get; set; }

        [ForeignKey(nameof(ProgramID))]
        public EduProgram? Program { get; set; }

        [ForeignKey(nameof(SubjectID))]
        public EduSubject? Subject { get; set; }
    }
}
