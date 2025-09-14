using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Interfaces
{
    public interface ICredentialRepository : IRepository<AppCredential>
    {
        Task<AppCredential> GetByName(string name);
    }
}
