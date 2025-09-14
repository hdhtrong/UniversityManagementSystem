using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduCurriculumService
    {
        Task<EduCurriculum> GetById(Guid programId, Guid subjectId);
        Task<IEnumerable<EduCurriculum>> GetAll();
        Task<IEnumerable<EduCurriculum>> GetSubjectsByProgram(Guid programId);
        Task<IEnumerable<EduCurriculum>> GetSubjectsByProgramAndSemester(Guid programId, int semesterOrder);
        IQueryable<EduCurriculum> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduCurriculum entity);
        Task<bool> Update(EduCurriculum entity);
        Task<bool> Delete(Guid programId, Guid subjectId);
    }
}
