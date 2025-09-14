using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduInstructorRepository : Repository<EduInstructor>, IEduInstructorRepository
    {
        public EduInstructorRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
