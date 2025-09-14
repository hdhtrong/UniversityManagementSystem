using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduTuitionFeeService
    {
        Task<EduTuitionFee> GetById(Guid id);
        Task<IEnumerable<EduTuitionFee>> GetAll();
        IQueryable<EduTuitionFee> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduTuitionFee entity);
        Task<bool> Update(EduTuitionFee entity);
        Task<bool> Delete(Guid id);
    }
}
