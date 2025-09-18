using HRMService.Domain.Entities;
using HRMService.Infrastructure;
using Shared.SharedKernel.Models;
using HRMService.Services.Interfaces;

namespace HRMService.Application.Services.Implementations
{
    public class HrmEmployeeService : IHrmEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HrmEmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(HrmEmployee e)
        {
            if (e != null)
            {
                await _unitOfWork.EmployeeRepository.Add(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var s = await _unitOfWork.EmployeeRepository.GetById(id);
            if (s != null)
            {
                _unitOfWork.EmployeeRepository.Delete(s);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<HrmEmployee>> GetAll()
        {
            return await _unitOfWork.EmployeeRepository.GetAll();
        }

        public IQueryable<HrmEmployee> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "Fullname", "Gender", "DOB", "Email", "Code", "IdentityNumber", "Nationality", "DepartmentID" };
            return _unitOfWork.EmployeeRepository.GetByFilter(filter, out total, allowedFields, null);
        }

        public async Task<HrmEmployee> GetById(Guid id)
        {
            return await _unitOfWork.EmployeeRepository.GetById(id);
        }

        public async Task<bool> Update(HrmEmployee e)
        {
            if (e != null)
            {
                _unitOfWork.EmployeeRepository.Update(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
