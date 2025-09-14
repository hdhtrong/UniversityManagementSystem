using EduService.Domain.Entities;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduCurriculumRepository : IRepository<EduCurriculum>
    {
        Task<EduCurriculum> GetById(Guid curriculumId, Guid programId);
    }
}
