using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduPeriodService : IEduPeriodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduPeriodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduPeriod entity)
        {
            if (entity != null)
            {
                await _unitOfWork.PeriodRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var e = await _unitOfWork.PeriodRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.PeriodRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduPeriod>> GetAll()
        {
            return await _unitOfWork.PeriodRepository.GetAll();
        }

        public IQueryable<EduPeriod> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.PeriodRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduPeriod> GetById(int id)
        {
            return await _unitOfWork.PeriodRepository.GetById(id);
        }

        public async Task<bool> Update(EduPeriod entity)
        {
            if (entity != null)
            {
                _unitOfWork.PeriodRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }

}
