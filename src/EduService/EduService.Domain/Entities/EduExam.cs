using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("Exams")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduExam : AuditableEntity
    {
        [Key]
        public Guid ExamID { get; set; }

        public Guid? SectionID { get; set; }

        [ForeignKey("SectionID")]
        public EduCourseSection? Section { get; set; }

        public int? Attendees { get; set; }

        public Guid? ExamSessionID { get; set; }

        [ForeignKey("ExamSessionID")]
        public EduExamSession? ExamSession { get; set; }

        public DateTime? ExamDate { get; set; }

        public int? StartPeriod { get; set; }

        public int? EndPeriod { get; set; }

        [MaxLength(100)]
        public string? Code { get; set; } // CourseSectionCode-ExamSessionCode

        [MaxLength(250)]
        public string? Note { get; set; }

        public Guid? RoomID { get; set; }

        [ForeignKey("RoomID")]
        public EduRoom? Room { get; set; }
    }
}
