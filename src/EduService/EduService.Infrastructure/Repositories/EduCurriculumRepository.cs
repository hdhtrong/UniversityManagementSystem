using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduCurriculumRepository : Repository<EduCurriculum>, IEduCurriculumRepository
    {
        public EduCurriculumRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduCurriculum> GetById(Guid curriculumId, Guid programId)
        {
            return await _dbContext.Set<EduCurriculum>().FindAsync(curriculumId, programId);
        }
    }
}
