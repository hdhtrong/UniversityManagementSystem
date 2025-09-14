using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Shared.SharedKernel.Models;
using Shared.SharedKernel.CustomQuery;

namespace HRMService.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HrmDbContext _dbContext;

        protected Repository(HrmDbContext context)
        {
            _dbContext = context;
        }

        public async Task<T> GetById(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }


        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public Task<T> GetSingleByConditions(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefaultAsync(predicate);
            }
            return _dbContext.Set<T>().Where(predicate).SingleOrDefaultAsync();
        }

        public IQueryable<T> GetMultiByConditions(Expression<Func<T, bool>> predicate, string[] includes = null)     
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where(predicate).AsQueryable();
            }
            return _dbContext.Set<T>().Where(predicate).AsQueryable();
        }

        public virtual IQueryable<T> GetMultiPagingByConditions(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> resultSet;

            // Join table
            if (includes != null && includes.Count() > 0)
            {
                var query = _dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                resultSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                resultSet = predicate != null ? _dbContext.Set<T>().Where<T>(predicate).AsQueryable() : _dbContext.Set<T>().AsQueryable();
            }
            total = resultSet.Count();

            // Take selected page
            IQueryable<T> pageResult = skipCount == 0 ? resultSet.Take(size) : resultSet.Skip(skipCount).Take(size);
            return pageResult.AsQueryable();
        }

        public IQueryable<T> GetByFilter(FilterRequest filter, out int total, HashSet<string>? allowedFields = null, string[]? includes = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null && includes.Any())
            {
                query = query.Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
            }

            return query.AsQueryable().ApplyFilterPaging(filter, out total, allowedFields);
        }
    }
}
