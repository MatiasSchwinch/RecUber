using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecUber.Interface
{
    public interface IRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetByPredicate(Func<T, bool> predicate);
        Task<T> GetById(int id);
        Task Insert(T entity);
        Task Delete(int id);
        Task Save();
    }
}
