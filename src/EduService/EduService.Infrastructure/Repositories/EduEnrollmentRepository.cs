using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduEnrollmentRepository : Repository<EduEnrollment>, IEduEnrollmentRepository
    {
        public EduEnrollmentRepository(EduDbContext dbContext) : base(dbContext) { }
    }
}
