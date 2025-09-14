namespace EduService.API.Models
{
    public class EduSubjectPrerequisiteDto
    {
        public Guid SubjectID { get; set; }
        public Guid PrerequisiteSubjectID { get; set; }
        public int? Order { get; set; }
        public string? Type { get; set; }
        public string? Note { get; set; }
    }
}
