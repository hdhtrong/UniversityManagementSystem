using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduScheduleWeekRepository : Repository<EduScheduleWeek>, IEduScheduleWeekRepository
    {
        public EduScheduleWeekRepository(EduDbContext dbContext) : base(dbContext) { }

        public async Task<EduScheduleWeek?> GetById(Guid scheduleId, Guid weekId)
        {
            // EF Core FindAsync hỗ trợ composite key theo đúng thứ tự khai báo HasKey / [PrimaryKey]
            return await _dbContext.Set<EduScheduleWeek>().FindAsync(scheduleId, weekId);
        }

        public async Task Delete(Guid scheduleId, Guid weekId)
        {
            var entity = await GetById(scheduleId, weekId);
            if (entity != null)
            {
                // Tận dụng Delete(entity) từ base Repository<>
                Delete(entity);
            }
        }

        public async Task<bool> Exists(Guid scheduleId, Guid weekId)
        {
            return await _dbContext.Set<EduScheduleWeek>()
                .AsNoTracking()
                .AnyAsync(x => x.ScheduleID == scheduleId && x.WeekID == weekId);
        }
    }
}
