using HRMService.Domain.Entities;
using HRMService.Infrastructure.Interfaces;

namespace HRMService.Infrastructure.Repositories
{
    public class HrmDepartmentRepository : Repository<HrmDepartment>, IHrmDepartmentRepository
    {
        public HrmDepartmentRepository(HrmDbContext dbContext) : base(dbContext)
        {

        }
    }
}
