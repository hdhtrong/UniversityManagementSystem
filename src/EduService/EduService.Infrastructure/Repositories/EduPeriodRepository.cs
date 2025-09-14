using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduPeriodRepository : Repository<EduPeriod>, IEduPeriodRepository
    {
        public EduPeriodRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduPeriod> GetById(int id)
        {
            return await _dbContext.Set<EduPeriod>().FindAsync(id);
        }
    }
}
