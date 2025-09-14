using Shared.SharedKernel.CustomQuery;
using Shared.SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EduService.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        Task<T?> FindByKeys(params object[] keys);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetSingleByConditions(Expression<Func<T, bool>> predicate, string[] includes = null);
        IQueryable<T> GetMultiByConditions(Expression<Func<T, bool>> predicate, string[] includes = null);
        IQueryable<T> GetMultiPagingByConditions(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null);
        IQueryable<T> GetByFilter(FilterRequest filter, out int total, HashSet<string>? allowedFields = null, string[]? includes = null);
    }
}
