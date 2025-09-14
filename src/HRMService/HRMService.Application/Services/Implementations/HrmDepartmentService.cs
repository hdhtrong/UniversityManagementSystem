using HRMService.Domain.Entities;
using HRMService.Infrastructure;
using Shared.SharedKernel.Models;
using HRMService.Services.Interfaces;

namespace HRMService.Application.Services.Implementations
{
    public class HrmDepartmentService : IHrmDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HrmDepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(HrmDepartment st)
        {
            if (st != null)
            {
                await _unitOfWork.DepartmentRepository.Add(st);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var s = await _unitOfWork.DepartmentRepository.GetById(id);
            if (s != null)
            {
                _unitOfWork.DepartmentRepository.Delete(s);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<HrmDepartment>> GetAll()
        {
            return await _unitOfWork.DepartmentRepository.GetAll();
        }

        public IQueryable<HrmDepartment> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.DepartmentRepository.GetByFilter(filter, out total, null);
        }

        public async Task<HrmDepartment> GetById(Guid id)
        {
            return await _unitOfWork.DepartmentRepository.GetById(id);
        }

        public async Task<bool> Update(HrmDepartment s)
        {
            if (s != null)
            {
                _unitOfWork.DepartmentRepository.Update(s);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
