using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduInstructorService : IEduInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduInstructorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduInstructor entity)
        {
            if (entity != null)
            {
                await _unitOfWork.InstructorRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.InstructorRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.InstructorRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduInstructor>> GetAll()
        {
            return await _unitOfWork.InstructorRepository.GetAll();
        }

        public IEnumerable<EduInstructor> GetByDepartmentId(Guid deptId)
        {
            return _unitOfWork.InstructorRepository.GetMultiByConditions(i => i.DepartmentID == deptId).OrderBy(i => i.Code);
        }

        public IQueryable<EduInstructor> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.InstructorRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduInstructor> GetById(Guid id)
        {
            return await _unitOfWork.InstructorRepository.GetById(id);
        }

        public async Task<bool> Update(EduInstructor entity)
        {
            if (entity != null)
            {
                _unitOfWork.InstructorRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
