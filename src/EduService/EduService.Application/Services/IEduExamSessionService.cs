using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduExamSessionService
    {
        Task<EduExamSession> GetById(Guid id);
        Task<IEnumerable<EduExamSession>> GetAll();
        IQueryable<EduExamSession> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduExamSession entity);
        Task<bool> Update(EduExamSession entity);
        Task<bool> Delete(Guid id);
    }
}
