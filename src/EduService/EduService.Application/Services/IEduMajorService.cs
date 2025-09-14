using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduMajorService
    {
        Task<EduMajor> GetById(Guid id);
        Task<IEnumerable<EduMajor>> GetAll();
        IQueryable<EduMajor> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduMajor entity);
        Task<bool> Update(EduMajor entity);
        Task<bool> Delete(Guid id);
    }

}
