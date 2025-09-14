using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduAcademicYearService : IEduAcademicYearService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduAcademicYearService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduAcademicYear entity)
        {
            if (entity != null)
            {
                await _unitOfWork.AcademicYearRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.AcademicYearRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.AcademicYearRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduAcademicYear>> GetAll()
        {
            return await _unitOfWork.AcademicYearRepository.GetAll();
        }

        public IQueryable<EduAcademicYear> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.AcademicYearRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduAcademicYear> GetById(Guid id)
        {
            return await _unitOfWork.AcademicYearRepository.GetById(id);
        }

        public async Task<EduAcademicYear> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.AcademicYearRepository.GetByIdAsync(id);
        }

        public async Task<bool> Update(EduAcademicYear entity)
        {
            if (entity != null)
            {
                _unitOfWork.AcademicYearRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }

}
