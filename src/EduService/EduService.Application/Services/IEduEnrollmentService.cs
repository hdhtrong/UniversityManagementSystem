using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduEnrollmentService
    {
        Task<EduEnrollment> GetById(Guid id);
        Task<IEnumerable<EduEnrollment>> GetAll();
        IQueryable<EduEnrollment> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduEnrollment entity);
        Task<bool> Update(EduEnrollment entity);
        Task<bool> Delete(Guid id);
    }
}
