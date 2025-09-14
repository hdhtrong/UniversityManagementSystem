using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class CredentialRepository : Repository<AppCredential>, ICredentialRepository
    {
        public CredentialRepository(AuthDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<AppCredential> GetByName(string name)
        {
            return await _dbContext.AppCredentials.Where(c => c.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
