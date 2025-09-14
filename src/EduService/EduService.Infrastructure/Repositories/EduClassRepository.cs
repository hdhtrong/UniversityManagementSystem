using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;


namespace EduService.Infrastructure.Repositories
{
    public class EduClassRepository : Repository<EduClass>, IEduClassRepository
    {
        public EduClassRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
