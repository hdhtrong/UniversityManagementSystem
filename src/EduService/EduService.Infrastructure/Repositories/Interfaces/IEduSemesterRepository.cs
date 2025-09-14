using EduService.Domain.Entities;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduSemesterRepository : IRepository<EduSemester>
    {
        Task<EduSemester?> GetByIdAsync(Guid id);
    }
}
