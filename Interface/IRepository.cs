﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecUber.Interface
{
    public interface IRepository<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Insert(T entity);
        Task Delete(int id);
        void Update(T entity);
        Task Save();
    }
}