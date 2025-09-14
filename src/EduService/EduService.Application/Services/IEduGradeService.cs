using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduGradeService
    {
        Task<EduGrade> GetById(Guid id);
        Task<IEnumerable<EduGrade>> GetAll();
        IQueryable<EduGrade> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduGrade entity);
        Task<bool> Update(EduGrade entity);
        Task<bool> Delete(Guid id);
    }
}
