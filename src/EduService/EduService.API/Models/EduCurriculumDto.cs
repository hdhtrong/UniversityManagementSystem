namespace EduService.API.Models
{
    public class EduCurriculumDto
    {
        public Guid ProgramID { get; set; }
        public Guid SubjectID { get; set; }
        public int SemesterOrder { get; set; }
        public bool Compulsory { get; set; }
    }
}
