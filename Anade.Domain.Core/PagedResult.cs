using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Domain.Core
{
    public class PagedResult<T>
    {
        public PagedResult(List<T> items, int totalCount, int startRowIndex, int maxRows)
        {
            Items = items;
            TotalCount = totalCount;
            Page = (int)Math.Ceiling((double)(startRowIndex / maxRows)) + 1;
            PageSize = maxRows;
        }
        public List<T> Items { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }
    }
}
