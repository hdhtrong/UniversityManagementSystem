using EduService.Domain.Entities;
using EduService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduStudentRepository : Repository<EduStudent>, IEduStudentRepository
    {
        public EduStudentRepository(EduDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> AddRangeAsync(IEnumerable<EduStudent> students)
        {
            await _dbContext.Set<EduStudent>().AddRangeAsync(students);
            return _dbContext.SaveChanges();
        }

        public async Task<bool> ExistsByStudentIdAsync(string studentId)
        {
            return await _dbContext.Set<EduStudent>().AnyAsync(st => st.StudentID.Equals(studentId));
        }

        public async Task<EduStudent?> GetByIdAsync(Guid studentId)
        {
            return await _dbContext.Set<EduStudent>()
                        .Include(s => s.Class)
                            .ThenInclude(c => c.Instructor) 
                        .Include(s => s.Class)
                            .ThenInclude(c => c.Program)
                                .ThenInclude(p => p.Major)
                                    .ThenInclude(m => m.Department)
                        .FirstOrDefaultAsync(s => s.ID == studentId);
        }
    }
}
