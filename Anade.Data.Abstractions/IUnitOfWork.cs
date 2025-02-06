using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Data.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<T, TKey> GetRepository<T, TKey>() where T : class, IEntity<TKey>;
        int SaveChanges();
    }
    public interface IUnitOfWork<TContext> : IUnitOfWork
    {

    }

}
