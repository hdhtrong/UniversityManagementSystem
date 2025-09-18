using HRMService.Domain.Entities;
using HRMService.Infrastructure.Repositories.Interfaces;

namespace HRMService.Infrastructure.Repositories
{
    public class HrmEmployeeRepository : Repository<HrmEmployee>, IHrmEmployeeRepository
    {
        public HrmEmployeeRepository(HrmDbContext dbContext) : base(dbContext)
        {

        }
    }
}
