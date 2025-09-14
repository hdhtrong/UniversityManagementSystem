using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduSemesterService : IEduSemesterService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduSemesterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduSemester entity)
        {
            if (entity != null)
            {
                await _unitOfWork.SemesterRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.SemesterRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.SemesterRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduSemester>> GetAll()
        {
            return await _unitOfWork.SemesterRepository.GetAll();
        }

        public IQueryable<EduSemester> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.SemesterRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduSemester> GetById(Guid id)
        {
            return await _unitOfWork.SemesterRepository.GetById(id);
        }

        public async Task<EduSemester> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.SemesterRepository.GetByIdAsync(id);
        }

        public async Task<EduSemester> GetCurrentSemester()
        {
            DateTime now = DateTime.Now;
            return await _unitOfWork.SemesterRepository.GetSingleByConditions(s => s.Activated.HasValue && s.Activated.Value && s.StartDate <= now && now <= s.EndDate, new string[] { "AcademicYear", "Weeks" });
        }

        public async Task<bool> Update(EduSemester entity)
        {
            if (entity != null)
            {
                _unitOfWork.SemesterRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }

}
