using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduClassService
    {
        Task<EduClass> GetById(Guid id);
        Task<IEnumerable<EduClass>> GetAll();
        IQueryable<EduClass> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduClass entity);
        Task<bool> Update(EduClass entity);
        Task<bool> Delete(Guid id);
    }
}
