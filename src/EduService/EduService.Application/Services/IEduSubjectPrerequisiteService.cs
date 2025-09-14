using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduSubjectPrerequisiteService
    {
        Task<EduSubjectPrerequisite> GetById(Guid subjectId, Guid prerequisiteSubjectId);
        Task<IEnumerable<EduSubjectPrerequisite>> GetAll();
        IQueryable<EduSubjectPrerequisite> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduSubjectPrerequisite entity);
        Task<bool> Update(EduSubjectPrerequisite entity);
        Task<bool> Delete(Guid subjectId, Guid prerequisiteSubjectId);
    }
}
