using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduSemesterRepository : Repository<EduSemester>, IEduSemesterRepository
    {
        public EduSemesterRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduSemester?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<EduSemester>().Include(y => y.Weeks)
                                                          .FirstOrDefaultAsync(y => y.SemesterID == id);
        }
    }
}
