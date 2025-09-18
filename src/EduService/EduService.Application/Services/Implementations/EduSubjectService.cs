using EduService.Domain.Entities;
using EduService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduSubjectService : IEduSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduSubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduSubject entity)
        {
            if (entity != null)
            {
                await _unitOfWork.SubjectRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.SubjectRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.SubjectRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduSubject>> GetAll()
        {
            return await _unitOfWork.SubjectRepository.GetAll();
        }

        public async Task<IEnumerable<EduSubject>> GetPrerequisitesAsync(Guid subjectId)
        {
            var prerequisites = await _unitOfWork
                .SubjectPrerequisiteRepository
                .GetMultiByConditions(
                    p => p.SubjectID == subjectId,
                    [nameof(EduSubjectPrerequisite.PrerequisiteSubject)]
                )
                .Select(p => p.PrerequisiteSubject!)
                .ToListAsync();

            return prerequisites;
        }

        public IQueryable<EduSubject> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "SubjectCode", "SubjectName", "SubjectID" };
            return _unitOfWork.SubjectRepository.GetByFilter(filter, out total, allowedFields, null);
        }

        public async Task<EduSubject> GetById(Guid id)
        {
            return await _unitOfWork.SubjectRepository.GetById(id);
        }

        public async Task<bool> Update(EduSubject entity)
        {
            if (entity != null)
            {
                _unitOfWork.SubjectRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
