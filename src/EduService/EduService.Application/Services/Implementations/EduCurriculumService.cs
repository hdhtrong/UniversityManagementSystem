using EduService.Domain.Entities;
using EduService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduCurriculumService : IEduCurriculumService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduCurriculumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduCurriculum entity)
        {
            if (entity != null)
            {
                await _unitOfWork.CurriculumRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid programId, Guid subjectId)
        {
            var e = await _unitOfWork.CurriculumRepository.GetById(programId, subjectId);
            if (e != null)
            {
                _unitOfWork.CurriculumRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduCurriculum>> GetAll()
        {
            return await _unitOfWork.CurriculumRepository.GetAll();
        }

        public IQueryable<EduCurriculum> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.CurriculumRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduCurriculum> GetById(Guid programId, Guid subjectId)
        {
            return await _unitOfWork.CurriculumRepository.GetById(programId, subjectId);
        }

        public async Task<IEnumerable<EduCurriculum>> GetSubjectsByProgram(Guid programId)
        {
            return await _unitOfWork.CurriculumRepository
                .GetMultiByConditions(c => c.ProgramID == programId, new[] { nameof(EduCurriculum.Subject) })
                .OrderBy(c => c.SemesterOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<EduCurriculum>> GetSubjectsByProgramAndSemester(Guid programId, int semesterOrder)
        {
            return await _unitOfWork.CurriculumRepository
                .GetMultiByConditions(c => c.ProgramID == programId && c.SemesterOrder == semesterOrder, new[] { nameof(EduCurriculum.Subject) })
                .ToListAsync();
        }

        public async Task<bool> Update(EduCurriculum entity)
        {
            if (entity != null)
            {
                _unitOfWork.CurriculumRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
