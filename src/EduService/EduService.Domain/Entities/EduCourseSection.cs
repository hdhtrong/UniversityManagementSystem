using SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EduService.Domain.Entities
{
    [Table("CourseSections")]
    [Index(nameof(Code), IsUnique = true)]
    public class EduCourseSection : AuditableEntity
    {
        [Key]
        public Guid SectionID { get; set; }

        public Guid? SubjectID { get; set; }

        [ForeignKey("SubjectID")]
        public EduSubject? Subject { get; set; }

        public Guid? SemesterID { get; set; }

        [ForeignKey("SemesterID")]
        public EduSemester? Semester { get; set; }

        public Guid? InstructorID { get; set; }

        [ForeignKey("InstructorID")]
        public EduInstructor? Instructor { get; set; }

        public int? MaxStudents { get; set; }

        [MaxLength(100)]
        public required string Code { get; set; } // SubjectCode-SemesterCode-Group

        public required int Group { get; set; }

        [MaxLength(256)]
        public string? Note { get; set; }

        public ICollection<EduSchedule>? Schedules { get; set; }
        public ICollection<EduEnrollment>? Enrollments { get; set; }
        public ICollection<EduExam>? Exams { get; set; }
    }
}
