using EduService.Domain.Entities;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduAcademicYearRepository : IRepository<EduAcademicYear>
    {
        Task<EduAcademicYear?> GetByIdAsync(Guid id);
    }
}
