using EduService.Domain.Entities;

namespace EduService.Infrastructure.Interfaces
{
    public interface IEduStudentRepository : IRepository<EduStudent>
    {
        Task<EduStudent?> GetByIdAsync(Guid Id);
        Task<bool> ExistsByStudentIdAsync(string studentId);
        Task<int> AddRangeAsync(IEnumerable<EduStudent> students);
    }
}
