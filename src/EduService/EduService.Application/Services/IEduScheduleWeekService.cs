using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduScheduleWeekService
    {
        Task<EduScheduleWeek> GetById(Guid scheduleId, Guid weekId);
        Task<IEnumerable<EduScheduleWeek>> GetAll();
        IQueryable<EduScheduleWeek> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduScheduleWeek entity);
        Task<bool> Update(EduScheduleWeek entity);
        Task<bool> Delete(Guid scheduleId, Guid weekId);
    }
}
