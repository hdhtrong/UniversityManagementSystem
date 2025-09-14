using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;


namespace EduService.Application.Services.Implementations
{
    public class EduWeekService : IEduWeekService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduWeekService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduWeek entity)
        {
            if (entity != null)
            {
                await _unitOfWork.WeekRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.WeekRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.WeekRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduWeek>> GetAll()
        {
            return await _unitOfWork.WeekRepository.GetAll();
        }

        public IQueryable<EduWeek> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.WeekRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduWeek> GetById(Guid id)
        {
            return await _unitOfWork.WeekRepository.GetById(id);
        }

        public async Task<EduWeek> GetCurrentWeek()
        {
            DateTime now = DateTime.Now;
            return await _unitOfWork.WeekRepository.GetSingleByConditions(s => s.StartDate <= now && now <= s.EndDate, new string[] { "Semester", "Semester.AcademicYear" });
        }

        public async Task<bool> Update(EduWeek entity)
        {
            if (entity != null)
            {
                _unitOfWork.WeekRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }

}
