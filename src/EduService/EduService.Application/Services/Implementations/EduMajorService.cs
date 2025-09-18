using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduMajorService : IEduMajorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduMajorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduMajor entity)
        {
            if (entity != null)
            {
                await _unitOfWork.MajorRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.MajorRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.MajorRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduMajor>> GetAll()
        {
            return await _unitOfWork.MajorRepository.GetAll();
        }

        public IQueryable<EduMajor> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "MajorCode", "MajorName", "MajorType", "DepartmentID" };
            return _unitOfWork.MajorRepository.GetByFilter(filter, out total, allowedFields, ["Department", "Programs"]);
        }

        public async Task<EduMajor> GetById(Guid id)
        {
            return await _unitOfWork.MajorRepository.GetById(id);
        }

        public async Task<bool> Update(EduMajor entity)
        {
            if (entity != null)
            {
                _unitOfWork.MajorRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
