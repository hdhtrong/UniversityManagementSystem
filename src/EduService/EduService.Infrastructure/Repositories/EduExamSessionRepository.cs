using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduExamSessionRepository : Repository<EduExamSession>, IEduExamSessionRepository
    {
        public EduExamSessionRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
