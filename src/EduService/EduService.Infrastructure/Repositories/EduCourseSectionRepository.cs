using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories
{
    public class EduCourseSectionRepository : Repository<EduCourseSection>, IEduCourseSectionRepository
    {
        public EduCourseSectionRepository(EduDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<EduCourseSection?> GetByIdWithDetails(Guid id)
        {
            return await _dbContext.CourseSections
                          .Include(cs => cs.Subject)
                          .Include(cs => cs.Semester)
                          .Include(cs => cs.Instructor)
                          .Include(cs => cs.Schedules)
                              .ThenInclude(s => s.Room)
                          .Include(cs => cs.Schedules)
                              .ThenInclude(s => s.ScheduleWeeks)
                          .FirstOrDefaultAsync(cs => cs.SectionID == id);
        }
    }
}
