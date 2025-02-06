using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public interface ILoadProperties<T, TKey> where T : class, IEntity<TKey>
    {
        //public object[] GetPropertiesFromNavProp(List<T> entity, Type decision);

        //public Dictionary<string, object> GetListWithNames(List<T> entity, Type decision);


        //public List<string> GetAllNavsProperties(List<T> entity, Type decision);


        public Dictionary<string, object> GetFilteredNavProp(List<T> entity, Type decision);
        public Dictionary<string, object> GetFilteredNavProp(T entity, Type decision);
        public Dictionary<string, object> GetAllNavProp(List<T> entity);
        public Dictionary<string, object> GetAllNavProp(T entity);

    }
}
