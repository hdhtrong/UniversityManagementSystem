using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduScheduleWeekService : IEduScheduleWeekService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduScheduleWeekService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduScheduleWeek entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ScheduleWeekRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid scheduleId, Guid weekId)
        {
            var e = await _unitOfWork.ScheduleWeekRepository.GetById(scheduleId, weekId);
            if (e != null)
            {
                _unitOfWork.ScheduleWeekRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduScheduleWeek>> GetAll() =>
            await _unitOfWork.ScheduleWeekRepository.GetAll();

        public IQueryable<EduScheduleWeek> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.ScheduleWeekRepository.GetByFilter(filter, out total, null);

        public async Task<EduScheduleWeek> GetById(Guid scheduleId, Guid weekId) =>
            await _unitOfWork.ScheduleWeekRepository.GetById(scheduleId, weekId);

        public async Task<bool> Update(EduScheduleWeek entity)
        {
            if (entity != null)
            {
                _unitOfWork.ScheduleWeekRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
