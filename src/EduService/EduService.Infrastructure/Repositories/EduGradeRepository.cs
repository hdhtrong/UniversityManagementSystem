using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduGradeRepository : Repository<EduGrade>, IEduGradeRepository
    {
        public EduGradeRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}