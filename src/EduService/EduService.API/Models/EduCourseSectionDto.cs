namespace EduService.API.Models
{
    public class EduCourseSectionDto
    {
        public Guid SectionID { get; set; }
        public Guid? SubjectID { get; set; }
        public Guid? SemesterID { get; set; }
        public Guid? InstructorID { get; set; }
        public int? MaxStudents { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Group { get; set; }
        public string? Note { get; set; }
        
        // Display fields
        public string? SubjectName { get; set; }
        public string? SemesterName { get; set; }
        public string? InstructorName { get; set; }

        public List<EduScheduleDto>? Schedules { get; set; }
    }
}
