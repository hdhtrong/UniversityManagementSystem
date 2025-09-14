using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduExamRepository : Repository<EduExam>, IEduExamRepository
    {
        public EduExamRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}