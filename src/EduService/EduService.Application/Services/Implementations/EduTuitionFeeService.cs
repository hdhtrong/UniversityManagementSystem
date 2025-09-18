using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduTuitionFeeService : IEduTuitionFeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduTuitionFeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduTuitionFee entity)
        {
            if (entity != null)
            {
                await _unitOfWork.TuitionFeeRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.TuitionFeeRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.TuitionFeeRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduTuitionFee>> GetAll()
        {
            return await _unitOfWork.TuitionFeeRepository.GetAll();
        }

        public IQueryable<EduTuitionFee> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "Code", "StudentID", "SemesterID"};
            return _unitOfWork.TuitionFeeRepository.GetByFilter(filter, out total, allowedFields, ["Student", "Semester", "Invoices"]);
        }

        public async Task<EduTuitionFee> GetById(Guid id)
        {
            return await _unitOfWork.TuitionFeeRepository.GetById(id);
        }

        public async Task<bool> Update(EduTuitionFee entity)
        {
            if (entity != null)
            {
                _unitOfWork.TuitionFeeRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
