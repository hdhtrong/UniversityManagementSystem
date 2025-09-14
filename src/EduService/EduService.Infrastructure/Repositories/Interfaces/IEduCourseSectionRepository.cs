using EduService.Domain.Entities;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduCourseSectionRepository : IRepository<EduCourseSection>
    {
        Task<EduCourseSection?> GetByIdWithDetails(Guid id);
    }
}