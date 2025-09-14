using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduAttendanceService
    {
        Task<EduAttendance> GetById(Guid id);
        Task<IEnumerable<EduAttendance>> GetAll();
        IQueryable<EduAttendance> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduAttendance entity);
        Task<bool> Update(EduAttendance entity);
        Task<bool> Delete(Guid id);
    }
}
