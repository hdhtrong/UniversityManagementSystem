using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduProgramService
    {
        Task<EduProgram> GetById(Guid id);
        Task<IEnumerable<EduProgram>> GetAll();
        IQueryable<EduProgram> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduProgram entity);
        Task<bool> Update(EduProgram entity);
        Task<bool> Delete(Guid id);
    }

}
