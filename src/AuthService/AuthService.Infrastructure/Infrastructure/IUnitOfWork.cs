using AuthService.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ICredentialRepository CredentialRepository { get; }
        int Save();
    }
}
