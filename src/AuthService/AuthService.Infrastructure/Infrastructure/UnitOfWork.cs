

using AuthService.Infrastructure.Interfaces;

namespace AuthService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _dbContext;

        public ICredentialRepository CredentialRepository { get; }

        public UnitOfWork(AuthDbContext dbContext, ICredentialRepository credentialRepository)
        {
            _dbContext = dbContext;
            CredentialRepository = credentialRepository;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }

}
