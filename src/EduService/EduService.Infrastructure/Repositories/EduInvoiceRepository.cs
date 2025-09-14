using EduService.Domain.Entities;
using EduService.Infrastructure.Repositories.Interfaces;

namespace EduService.Infrastructure.Repositories
{
    public class EduInvoiceRepository : Repository<EduInvoice>, IEduInvoiceRepository
    {
        public EduInvoiceRepository(EduDbContext dbContext) : base(dbContext)
        {
        }
    }
}
