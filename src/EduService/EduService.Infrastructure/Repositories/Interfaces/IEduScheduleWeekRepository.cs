using EduService.Domain.Entities;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduScheduleWeekRepository : IRepository<EduScheduleWeek> {
        Task<EduScheduleWeek?> GetById(Guid scheduleId, Guid weekId);
        Task Delete(Guid scheduleId, Guid weekId);
        Task<bool> Exists(Guid scheduleId, Guid weekId);
    }
}
