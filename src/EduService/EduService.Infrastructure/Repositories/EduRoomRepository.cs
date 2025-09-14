using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduRoomRepository : Repository<EduRoom>, IEduRoomRepository
    {
        public EduRoomRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
