using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduClassService : IEduClassService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduClassService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduClass entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ClassRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.ClassRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.ClassRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduClass>> GetAll()
        {
            return await _unitOfWork.ClassRepository.GetAll();
        }

        public IQueryable<EduClass> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "ClassCode", "ClassName", "StartYear", "ProgramID", "InstructorID" };
            return _unitOfWork.ClassRepository.GetByFilter(filter, out total, allowedFields, ["Program", "Instructor"]);
        }

        public async Task<EduClass> GetById(Guid id)
        {
            return await _unitOfWork.ClassRepository.GetById(id);
        }

        public async Task<bool> Update(EduClass entity)
        {
            if (entity != null)
            {
                _unitOfWork.ClassRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
