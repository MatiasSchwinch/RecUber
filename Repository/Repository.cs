using Microsoft.EntityFrameworkCore;
using RecUber.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecUber.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DatabaseConnection _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DatabaseConnection context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetByPredicate(Func<T, bool> predicate)
        {
            if (predicate is null) throw new Exception("El predicado no puede ser nulo");

            IEnumerable<T> list = new List<T>();

            await Task.Run(() =>
            {
                list = _dbSet.Where(predicate);
            });

            return list;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id) ?? null!;
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _dbSet.Remove(entity ?? throw new Exception("No se ha encontrado ninguna entidad con el identificador indicado."));
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        #region Implementación IDisposable
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
