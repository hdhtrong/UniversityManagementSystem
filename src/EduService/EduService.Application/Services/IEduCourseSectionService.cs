using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduCourseSectionService
    {
        Task<EduCourseSection> GetById(Guid id);
        Task<IEnumerable<EduCourseSection>> GetAll();
        IQueryable<EduCourseSection> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduCourseSection entity);
        Task<bool> Update(EduCourseSection entity);
        Task<bool> Delete(Guid id);
    }
}
