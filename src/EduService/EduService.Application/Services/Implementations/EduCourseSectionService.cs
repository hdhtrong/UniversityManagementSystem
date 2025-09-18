using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduCourseSectionService : IEduCourseSectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EduCourseSectionService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Create(EduCourseSection entity)
        {
            if (entity != null)
            {
                await _unitOfWork.CourseSectionRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.CourseSectionRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.CourseSectionRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduCourseSection>> GetAll() =>
            await _unitOfWork.CourseSectionRepository.GetAll();

        public IQueryable<EduCourseSection> GetByFilterPaging(FilterRequest filter, out int total)
        {
            var allowedFields = new HashSet<string> { "Code", "Group", "SubjectID", "SemesterID", "InstructorID" };
            return _unitOfWork.CourseSectionRepository.GetByFilter(filter, out total, allowedFields, ["Subject", "Semester", "Instructor"]);
        }
            
        public async Task<EduCourseSection> GetById(Guid id) =>
            await _unitOfWork.CourseSectionRepository.GetByIdWithDetails(id);

        public async Task<bool> Update(EduCourseSection entity)
        {
            if (entity != null)
            {
                _unitOfWork.CourseSectionRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
