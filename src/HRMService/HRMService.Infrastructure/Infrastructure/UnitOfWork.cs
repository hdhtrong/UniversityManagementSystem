
using HRMService.Infrastructure.Interfaces;
using HRMService.Infrastructure.Repositories.Interfaces;

namespace HRMService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HrmDbContext _dbContext;

        public IHrmDepartmentRepository DepartmentRepository { get; }
        public IHrmEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(HrmDbContext dbContext, IHrmDepartmentRepository deptRepository, IHrmEmployeeRepository employeeRepository)
        {
            _dbContext = dbContext;
            DepartmentRepository = deptRepository;
            EmployeeRepository = employeeRepository;
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
