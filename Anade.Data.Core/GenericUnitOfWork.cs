using Anade.Data.Abstractions;
using Anade.Domain.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Anade.Data.Core
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>
        {
            if (!_repositories.ContainsKey(typeof(T)))
                _repositories[typeof(T)] = new GenericRepository<T, TKey>(_dbContext);

            return _repositories[typeof(T)] as IRepository<T, TKey>;
        }



    }
}
