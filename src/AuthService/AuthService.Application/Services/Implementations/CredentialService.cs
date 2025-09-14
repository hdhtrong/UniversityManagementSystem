using AuthService.Domain.Entities;
using AuthService.Infrastructure;

namespace AuthService.Application.Services.Implementations
{
    public class CredentialService : ICredentialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CredentialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AppCredential>> GetAllAsync()
        {
            return await _unitOfWork.CredentialRepository.GetAll();
        }

        public async Task<AppCredential> GetByNameAsync(string name)
        {
            return await _unitOfWork.CredentialRepository.GetByName(name);
        }

        public async Task<bool> CreateAsync(AppCredential entity)
        {
            if (entity == null) return false;

            await _unitOfWork.CredentialRepository.Add(entity);
            return _unitOfWork.Save() > 0;
        }

        public async Task<bool> UpdateAsync(string name, AppCredential updatedEntity)
        {
            var existing = await _unitOfWork.CredentialRepository.GetByName(name);
            if (existing == null) return false;

            existing.Description = updatedEntity.Description;
            existing.Category = updatedEntity.Category;

            _unitOfWork.CredentialRepository.Update(existing);
            return _unitOfWork.Save() > 0;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            var existing = await _unitOfWork.CredentialRepository.GetByName(name);
            if (existing == null) return false;

            _unitOfWork.CredentialRepository.Delete(existing);
            return _unitOfWork.Save() > 0;
        }
    }

}
