using Anade.Data.Abstractions;
using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public static class EntityExtensions
    {

        public static object LoadProperties<T, TKey>(this T entity, string Prop, IRepository<T, TKey> repository)
              where T : class, IEntity<TKey>
        {


            return repository.LoadProperties(entity, Prop);

        }
    }
}
