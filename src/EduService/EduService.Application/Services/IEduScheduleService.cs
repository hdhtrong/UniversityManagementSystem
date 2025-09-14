using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduScheduleService
    {
        Task<EduSchedule> GetById(Guid id);
        Task<IEnumerable<EduSchedule>> GetAll();
        IQueryable<EduSchedule> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduSchedule entity);
        Task<bool> Update(EduSchedule entity);
        Task<bool> Delete(Guid id);
    }
}
