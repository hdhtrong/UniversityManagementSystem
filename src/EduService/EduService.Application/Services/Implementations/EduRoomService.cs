using EduService.Domain.Entities;
using EduService.Infrastructure;
using Shared.SharedKernel.Models;

namespace EduService.Application.Services.Implementations
{
    public class EduRoomService : IEduRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EduRoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Create(EduRoom entity)
        {
            if (entity != null)
            {
                await _unitOfWork.RoomRepository.Add(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var e = await _unitOfWork.RoomRepository.GetById(id);
            if (e != null)
            {
                _unitOfWork.RoomRepository.Delete(e);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<EduRoom>> GetAll()
        {
            return await _unitOfWork.RoomRepository.GetAll();
        }

        public IQueryable<EduRoom> GetByFilterPaging(FilterRequest filter, out int total)
        {
            return _unitOfWork.RoomRepository.GetByFilter(filter, out total, null);
        }

        public async Task<EduRoom> GetById(Guid id)
        {
            return await _unitOfWork.RoomRepository.GetById(id);
        }

        public async Task<bool> Update(EduRoom entity)
        {
            if (entity != null)
            {
                _unitOfWork.RoomRepository.Update(entity);
                return _unitOfWork.Save() > 0;
            }
            return false;
        }
    }
}
