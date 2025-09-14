using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduAcademicYearRepository : Repository<EduAcademicYear>, IEduAcademicYearRepository
    {
        public EduAcademicYearRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduAcademicYear?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<EduAcademicYear>().Include(y => y.Semesters)
                                                          .FirstOrDefaultAsync(y => y.YearID == id);
        }
    }
}
