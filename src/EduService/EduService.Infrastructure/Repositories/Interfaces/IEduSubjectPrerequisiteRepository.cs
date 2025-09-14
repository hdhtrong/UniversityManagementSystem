using EduService.Domain.Entities;


namespace EduService.Infrastructure.Repositories.Interfaces
{
    public interface IEduSubjectPrerequisiteRepository : IRepository<EduSubjectPrerequisite>
    {
        Task<EduSubjectPrerequisite> GetById(Guid subjectId, Guid prerequisiteId);
    }
}
