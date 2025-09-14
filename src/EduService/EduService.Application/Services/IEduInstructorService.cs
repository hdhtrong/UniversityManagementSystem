using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduInstructorService
    {
        Task<EduInstructor> GetById(Guid id);
        IEnumerable<EduInstructor> GetByDepartmentId(Guid deptId);
        Task<IEnumerable<EduInstructor>> GetAll();
        IQueryable<EduInstructor> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduInstructor entity);
        Task<bool> Update(EduInstructor entity);
        Task<bool> Delete(Guid id);

    }

}
