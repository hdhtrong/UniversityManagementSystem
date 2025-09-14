using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduWeekRepository : Repository<EduWeek>, IEduWeekRepository
    {
        public EduWeekRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
