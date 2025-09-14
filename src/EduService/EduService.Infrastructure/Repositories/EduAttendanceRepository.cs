using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduAttendanceRepository : Repository<EduAttendance>, IEduAttendanceRepository
    {
        public EduAttendanceRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}