using HRMService.Infrastructure.Interfaces;

namespace HRMService.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IHrmDepartmentRepository DepartmentRepository { get; }
        int Save();
    }
}
