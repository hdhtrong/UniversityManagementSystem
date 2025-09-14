using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduGradeService : IEduGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduGradeService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduGrade entity)
        {
            if (entity != null)
            {
                await _unitOfWork.GradeRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.GradeRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.GradeRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduGrade>> GetAll() =>
            await _unitOfWork.GradeRepository.GetAll();

        public IQueryable<EduGrade> GetByFilterPaging(FilterRequest filter, out int total) =>
            _unitOfWork.GradeRepository.GetByFilter(filter, out total, null);

        public async Task<EduGrade> GetById(Guid id) =>
            await _unitOfWork.GradeRepository.GetById(id);

        public async Task<bool> Update(EduGrade entity)
        {
            if (entity != null)
            {
                _unitOfWork.GradeRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
