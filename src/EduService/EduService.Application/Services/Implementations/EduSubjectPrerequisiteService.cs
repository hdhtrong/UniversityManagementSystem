using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduSubjectPrerequisiteService : IEduSubjectPrerequisiteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduSubjectPrerequisiteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduSubjectPrerequisite entity)
        {
            if (entity != null)
            {
                await _unitOfWork.SubjectPrerequisiteRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid subjectId, Guid prerequisiteSubjectId)
        {
            var e = await _unitOfWork.SubjectPrerequisiteRepository.GetById(subjectId, prerequisiteSubjectId);
            if (e != null)
            {
                _unitOfWork.SubjectPrerequisiteRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduSubjectPrerequisite>> GetAll()
        {
            return await _unitOfWork.SubjectPrerequisiteRepository.GetAll();
        }

        public IQueryable<EduSubjectPrerequisite> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.SubjectPrerequisiteRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduSubjectPrerequisite> GetById(Guid subjectId, Guid prerequisiteSubjectId)
        {
            return await _unitOfWork.SubjectPrerequisiteRepository.GetById(subjectId, prerequisiteSubjectId);
        }

        public async Task<bool> Update(EduSubjectPrerequisite entity)
        {
            if (entity != null)
            {
                _unitOfWork.SubjectPrerequisiteRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
