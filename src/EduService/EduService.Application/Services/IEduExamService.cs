using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduExamService
    {
        Task<EduExam> GetById(Guid id);
        Task<IEnumerable<EduExam>> GetAll();
        IQueryable<EduExam> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduExam entity);
        Task<bool> Update(EduExam entity);
        Task<bool> Delete(Guid id);
    }
}
