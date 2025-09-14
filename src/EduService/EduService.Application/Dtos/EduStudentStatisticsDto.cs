
namespace EduService.Application.Dtos
{
    public class EduStudentStatisticsDto
    {
        public int TotalCourses { get; set; }          // Số môn đã học
        public int TotalCredits { get; set; }          // Số tín chỉ đã học

        public int PassedCourses { get; set; }         // Số môn đậu
        public int PassedCredits { get; set; }         // Số tín chỉ đậu

        public int FailedCourses { get; set; }         // Số môn rớt
        public int FailedCredits { get; set; }         // Số tín chỉ rớt
    }

}
