using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduTuitionFeeRepository : Repository<EduTuitionFee>, IEduTuitionFeeRepository
    {
        public EduTuitionFeeRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
