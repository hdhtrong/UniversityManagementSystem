using EduService.Domain.Entities;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services
{
    public interface IEduRoomService
    {
        Task<EduRoom> GetById(Guid id);
        Task<IEnumerable<EduRoom>> GetAll();
        IQueryable<EduRoom> GetByFilterPaging(FilterRequest filter, out int total);
        Task<bool> Create(EduRoom entity);
        Task<bool> Update(EduRoom entity);
        Task<bool> Delete(Guid id);
    }
}
