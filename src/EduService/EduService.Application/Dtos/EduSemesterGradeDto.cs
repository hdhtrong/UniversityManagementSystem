
namespace EduService.Application.Dtos
{
    public class EduSemesterGradeDto
    {
        public string SemesterCode { get; set; } = null!;
        public string SemesterName { get; set; } = null!;
        public int SemesterOrder { get; set; } = 0!;
        public List<EduGradeItemDto> Items { get; set; } = new();
    }
}
