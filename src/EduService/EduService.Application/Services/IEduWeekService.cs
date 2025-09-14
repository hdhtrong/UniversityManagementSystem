using EduService.Domain.Entities;
using Shared.SharedKernel.Models;


namespace EduService.Application.Services
{
    public interface IEduWeekService
    {
        Task<EduWeek> GetById(Guid id);
        Task<IEnumerable<EduWeek>> GetAll();
        IQueryable<EduWeek> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduWeek entity);
        Task<bool> Update(EduWeek entity);
        Task<bool> Delete(Guid id);
        Task<EduWeek> GetCurrentWeek();
    }
}
