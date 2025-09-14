
using HRMService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace HRMService.Services.Interfaces
{
    public interface IHrmDepartmentService
    {
        Task<HrmDepartment> GetById(Guid id);

        Task<IEnumerable<HrmDepartment>> GetAll();

        IQueryable<HrmDepartment> GetByFilterPaging(FilterRequest filter, out int total);

        Task<bool> Create(HrmDepartment department);

        Task<bool> Update(HrmDepartment department);

        Task<bool> Delete(Guid id);
    }
}
