using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduSubjectService
    {
        Task<EduSubject> GetById(Guid id);
        Task<IEnumerable<EduSubject>> GetAll();
        Task<IEnumerable<EduSubject>> GetPrerequisitesAsync(Guid id);
        IQueryable<EduSubject> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduSubject entity);
        Task<bool> Update(EduSubject entity);
        Task<bool> Delete(Guid id);
    }
}
