using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduDepartmentRepository : Repository<EduDepartment>, IEduDepartmentRepository
    {
        public EduDepartmentRepository(EduDbContext dbContext) : base(dbContext)
        {

        }
    }
}
