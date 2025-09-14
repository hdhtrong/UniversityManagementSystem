using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduAttendanceService : IEduAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduAttendanceService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduAttendance entity)
        {
            if (entity != null)
            {
                await _unitOfWork.AttendanceRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.AttendanceRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.AttendanceRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduAttendance>> GetAll() =>
            await _unitOfWork.AttendanceRepository.GetAll();

        public IQueryable<EduAttendance> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.AttendanceRepository.GetByFilter(filter, out total, null);

        public async Task<EduAttendance> GetById(Guid id) =>
            await _unitOfWork.AttendanceRepository.GetById(id);

        public async Task<bool> Update(EduAttendance entity)
        {
            if (entity != null)
            {
                _unitOfWork.AttendanceRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
