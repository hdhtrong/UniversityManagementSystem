
namespace EduService.Application.Dtos
{
    public class EduSubjectWithPrereqDto
    {
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int SemesterOrder { get; set; }
        public bool Compulsory { get; set; }
        public List<EduPrerequisiteDto> Prerequisites { get; set; } = new();
    }
}
