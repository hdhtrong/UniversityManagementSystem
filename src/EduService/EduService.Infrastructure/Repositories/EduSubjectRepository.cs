using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduSubjectRepository : Repository<EduSubject>, IEduSubjectRepository
    {
        public EduSubjectRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
