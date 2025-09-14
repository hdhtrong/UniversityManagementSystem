using EduService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduPeriodRepository : IRepository<EduPeriod>
    {
         Task<EduPeriod> GetById(int id);     
    }
}
