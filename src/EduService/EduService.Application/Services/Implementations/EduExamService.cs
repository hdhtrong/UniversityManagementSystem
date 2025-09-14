using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduExamService : IEduExamService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduExamService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduExam entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ExamRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.ExamRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.ExamRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduExam>> GetAll() =>
            await _unitOfWork.ExamRepository.GetAll();

        public IQueryable<EduExam> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.ExamRepository.GetByFilter(filter, out total, null);

        public async Task<EduExam> GetById(Guid id) =>
            await _unitOfWork.ExamRepository.GetById(id);

        public async Task<bool> Update(EduExam entity)
        {
            if (entity != null)
            {
                _unitOfWork.ExamRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
