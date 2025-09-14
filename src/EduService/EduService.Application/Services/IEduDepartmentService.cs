using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduDepartmentService
    {
        Task<EduDepartment> GetById(Guid id);
        Task<IEnumerable<EduDepartment>> GetAll();
        IQueryable<EduDepartment> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduDepartment entity);
        Task<bool> Update(EduDepartment entity);
        Task<bool> Delete(Guid id);
    }
}
