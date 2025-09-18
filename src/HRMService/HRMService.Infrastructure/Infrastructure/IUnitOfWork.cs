using HRMService.Infrastructure.Interfaces;
using HRMService.Infrastructure.Repositories.Interfaces;

namespace HRMService.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IHrmDepartmentRepository DepartmentRepository { get; }
        IHrmEmployeeRepository EmployeeRepository { get; }
        int Save();
    }
}
