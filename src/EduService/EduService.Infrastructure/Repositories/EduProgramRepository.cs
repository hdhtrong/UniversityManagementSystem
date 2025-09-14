using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduProgramRepository : Repository<EduProgram>, IEduProgramRepository
    {
        public EduProgramRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
