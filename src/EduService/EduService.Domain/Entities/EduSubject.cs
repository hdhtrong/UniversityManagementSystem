using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace EduService.Domain.Entities
{
    [Table("Subjects")]
    [Index(nameof(SubjectCode), IsUnique = true)]
    public class EduSubject : AuditableEntity
    {
        [Key]
        public Guid SubjectID { get; set; }

        [MaxLength(20)]
        public string? SubjectCode { get; set; }

        [MaxLength(250)]
        public string? SubjectName { get; set; }

        public int Credits { get; set; }
        public int TheoryHours { get; set; }
        public int PracticeHours { get; set; }

        public int PercentageOfProgress { get; set; }
        public int PercentageOfHomework { get; set; }
        public int PercentageOfExam { get; set; }

        // Navigation properties
        public ICollection<EduSubjectPrerequisite>? Prerequisites { get; set; }
        public ICollection<EduSubjectPrerequisite>? IsPrerequisiteFor { get; set; }
        public ICollection<EduCurriculum>? Curriculums { get; set; }
        public ICollection<EduCourseSection>? CourseSections { get; set; }
    }
}
