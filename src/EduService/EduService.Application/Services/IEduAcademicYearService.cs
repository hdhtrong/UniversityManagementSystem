using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduAcademicYearService
    {
        Task<EduAcademicYear> GetById(Guid id);
        Task<EduAcademicYear> GetByIdAsync(Guid id);
        Task<IEnumerable<EduAcademicYear>> GetAll();
        IQueryable<EduAcademicYear> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduAcademicYear entity);
        Task<bool> Update(EduAcademicYear entity);
        Task<bool> Delete(Guid id);
    }
}
