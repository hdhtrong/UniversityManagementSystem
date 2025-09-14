using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduInvoiceService
    {
        Task<EduInvoice> GetById(Guid id);
        Task<IEnumerable<EduInvoice>> GetAll();
        IQueryable<EduInvoice> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduInvoice entity);
        Task<bool> Update(EduInvoice entity);
        Task<bool> Delete(Guid id);
    }
}
