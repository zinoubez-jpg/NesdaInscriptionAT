
using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Anade.Business.Core
{
    public interface IBusinessService<T,TKey> where T : class, IEntity<TKey>
    {
        T GetById(TKey id, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
        List<T> GetAll(params Expression<Func<T, object>>[] navigationPropertiesToLoad);
        PagedResult<T> GetAllPaged(string orderBy, int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
        List<T> GetAllFiltered(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertiesToLoad);
        PagedResult<T> GetAllFilteredPaged(Expression<Func<T, bool>> predicate, string orderBy, int startRowIndex = 1, int maxRows = 10, params Expression<Func<T, object>>[] navigationPropertiesToLoad);        
        BusinessResult Add(T entity);        
        BusinessResult Update(T entity);        
        BusinessResult Delete(T entity);
        
    }
}
