using HRMService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace HRMService.Services.Interfaces
{
    public interface IHrmEmployeeService
    {
        Task<HrmEmployee> GetById(Guid id);

        Task<IEnumerable<HrmEmployee>> GetAll();

        IQueryable<HrmEmployee> GetByFilterPaging(FilterRequest filter, out int total);

        Task<bool> Create(HrmEmployee e);

        Task<bool> Update(HrmEmployee e);

        Task<bool> Delete(Guid id);

    }
}
