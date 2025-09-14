using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Services
{
    public interface ICredentialService
    {
        Task<IEnumerable<AppCredential>> GetAllAsync();
        Task<AppCredential> GetByNameAsync(string name);
        Task<bool> CreateAsync(AppCredential dto);
        Task<bool> UpdateAsync(string name, AppCredential dto);
        Task<bool> DeleteAsync(string name);
    }
}
