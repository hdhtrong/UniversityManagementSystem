using EduService.Application.Dtos;
using EduService.Domain.Entities;

namespace EduService.Application.Services
{
    public interface IEduStudentPortalService
    {
        Task<EduStudentStatisticsDto> GetStatistics(string studentId);
        Task<EduStudentDetailDto> GetPersonalDetail(string studentId);
        Task<IEnumerable<EduWeekScheduleDto>> GetScheduleBySemester(string studentId, string semesterCode);
        Task<IEnumerable<EduSemesterGradeDto>> GetGradesBySemester(string studentId, string semesterCode);
        Task<IEnumerable<EduCurriculum>> GetCurriculums(string studentId);
        Task<IEnumerable<EduSubjectWithPrereqDto>> GetStudentPrerequisites(string studentId);
        Task<IEnumerable<EduSemester>> GetSemesters(string studentId);
        Task<IEnumerable<EduTuitionFee>> GetTuitions(string studentId);
        Task<IEnumerable<EduInvoice>> GetInvoices(string studentId);
    }
}
