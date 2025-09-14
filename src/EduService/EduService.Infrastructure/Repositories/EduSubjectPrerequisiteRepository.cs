using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduSubjectPrerequisiteRepository : Repository<EduSubjectPrerequisite>, IEduSubjectPrerequisiteRepository
    {
        public EduSubjectPrerequisiteRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduSubjectPrerequisite> GetById(Guid subjectId, Guid prerequisiteId)
        {
            return await _dbContext.Set<EduSubjectPrerequisite>().FindAsync(subjectId, prerequisiteId);
        }
    }
}
