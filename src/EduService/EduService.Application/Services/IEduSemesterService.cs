using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduSemesterService
    {
        Task<EduSemester> GetById(Guid id);
        Task<EduSemester> GetByIdAsync(Guid id);
        Task<IEnumerable<EduSemester>> GetAll();
        IQueryable<EduSemester> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduSemester entity);
        Task<bool> Update(EduSemester entity);
        Task<bool> Delete(Guid id);
        Task<EduSemester> GetCurrentSemester();
    }
}
