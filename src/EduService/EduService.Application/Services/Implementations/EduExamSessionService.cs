using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduExamSessionService : IEduExamSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduExamSessionService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduExamSession entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ExamSessionRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.ExamSessionRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.ExamSessionRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduExamSession>> GetAll() =>
            await _unitOfWork.ExamSessionRepository.GetAll();

        public IQueryable<EduExamSession> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.ExamSessionRepository.GetByFilter(filter, out total, null);

        public async Task<EduExamSession> GetById(Guid id) =>
            await _unitOfWork.ExamSessionRepository.GetById(id);

        public async Task<bool> Update(EduExamSession entity)
        {
            if (entity != null)
            {
                _unitOfWork.ExamSessionRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
