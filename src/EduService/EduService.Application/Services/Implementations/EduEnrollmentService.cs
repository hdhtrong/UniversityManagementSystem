using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;


namespace EduService.Application.Services.Implementations
{
    public class EduEnrollmentService : IEduEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduEnrollmentService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduEnrollment entity)
        {
            if (entity != null)
            {
                await _unitOfWork.EnrollmentRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.EnrollmentRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.EnrollmentRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduEnrollment>> GetAll() =>
            await _unitOfWork.EnrollmentRepository.GetAll();

        public IQueryable<EduEnrollment> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> {"StudentID", "SectionID" };
            return _unitOfWork.EnrollmentRepository.GetByFilter(filter, out total, allowedFields, ["Student", "Section", "Section.Subject", "Section.Semester", "Section.Instructor"]);
        }

        public async Task<EduEnrollment> GetById(Guid id) =>
            await _unitOfWork.EnrollmentRepository.GetById(id);

        public async Task<bool> Update(EduEnrollment entity)
        {
            if (entity != null)
            {
                _unitOfWork.EnrollmentRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
