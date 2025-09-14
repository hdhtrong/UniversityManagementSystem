using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduScheduleRepository : Repository<EduSchedule>, IEduScheduleRepository
    {
        public EduScheduleRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
