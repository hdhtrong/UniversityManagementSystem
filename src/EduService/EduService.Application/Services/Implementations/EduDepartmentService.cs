using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduDepartmentService : IEduDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduDepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduDepartment entity)
        {
            if (entity != null)
            {
                await _unitOfWork.DepartmentRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.DepartmentRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.DepartmentRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduDepartment>> GetAll()
        {
            return await _unitOfWork.DepartmentRepository.GetAll();
        }

        public IQueryable<EduDepartment> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.DepartmentRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduDepartment> GetById(Guid id)
        {
            return await _unitOfWork.DepartmentRepository.GetById(id);
        }

        public async Task<bool> Update(EduDepartment entity)
        {
            if (entity != null)
            {
                _unitOfWork.DepartmentRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
