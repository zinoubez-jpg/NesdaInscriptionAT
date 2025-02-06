using Anade.Business.Core;
using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public interface IApiService<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<PagedResult<T>> GetAllPagedAsync(string term, string orderBy, int page = 1, int pageSize = 10);
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<BusinessResult> PostAsync(T entity);
        Task<BusinessResult> PutAsync(int id, T entity);
        Task<BusinessResult> DeleteAsync(int id, T entity);

    }

}
