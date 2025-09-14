using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;


namespace EduService.Application.Services.Implementations
{
    public class EduScheduleService : IEduScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduScheduleService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduSchedule entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ScheduleRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.ScheduleRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.ScheduleRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduSchedule>> GetAll() =>
            await _unitOfWork.ScheduleRepository.GetAll();

        public IQueryable<EduSchedule> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.ScheduleRepository.GetByFilter(filter, out total, null);

        public async Task<EduSchedule> GetById(Guid id) =>
            await _unitOfWork.ScheduleRepository.GetById(id);

        public async Task<bool> Update(EduSchedule entity)
        {
            if (entity != null)
            {
                _unitOfWork.ScheduleRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
