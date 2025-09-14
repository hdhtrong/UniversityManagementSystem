using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduMajorRepository : Repository<EduMajor>, IEduMajorRepository
    {
        public EduMajorRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
