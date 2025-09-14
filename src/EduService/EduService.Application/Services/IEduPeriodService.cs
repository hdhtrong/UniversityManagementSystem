using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduPeriodService
    {
        Task<EduPeriod> GetById(int id);
        Task<IEnumerable<EduPeriod>> GetAll();
        IQueryable<EduPeriod> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduPeriod entity);
        Task<bool> Update(EduPeriod entity);
        Task<bool> Delete(int id);
    }
}
