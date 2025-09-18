using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduProgramService : IEduProgramService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduProgramService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduProgram entity)
        {
            if (entity != null)
            {
                await _unitOfWork.ProgramRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.ProgramRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.ProgramRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduProgram>> GetAll()
        {
            return await _unitOfWork.ProgramRepository.GetAll();
        }

        public IQueryable<EduProgram> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "ProgramName", "ProgramCode", "MajorID", "StartYear" };
            return _unitOfWork.ProgramRepository.GetByFilter(filter, out total, allowedFields, ["Major"]);
        }

        public async Task<EduProgram> GetById(Guid id)
        {
            return await _unitOfWork.ProgramRepository.GetById(id);
        }

        public async Task<bool> Update(EduProgram entity)
        {
            if (entity != null)
            {
                _unitOfWork.ProgramRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
