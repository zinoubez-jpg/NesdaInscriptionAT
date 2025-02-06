using Anade.Data.Abstractions;
using Anade.Domain.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Business.Core
{
    public class LoadProperties<T, TKey> : ILoadProperties<T, TKey> where T : class, IEntity<TKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<T, TKey> _repository;

        public LoadProperties(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T, TKey>();
        }

        //public List<string> GetAllNavsProperties(List<T> entity, Type decision)
        //{
        //    List<string> Getnav = Enum.GetNames(decision).ToList();
        //    string GetEntityName = entity.First().GetType().Name;
        //    Getnav.Add(GetEntityName);
        //    return Getnav;
        //}

        //public Dictionary<string, object> GetListWithNames(List<T> entity, Type decision) 
        //{
        //    Dictionary<string, object> NavPropWithProp = new Dictionary<string, object>();
        //    string[] GetNavProp = GetAllNavsProperties(entity, decision).ToArray();
        //    object[] GetListPro = GetPropertiesFromNavProp(entity, decision).ToArray();

        //    for (var i = 0; i < GetNavProp.ToArray().Length; i++)
        //    {

        //        NavPropWithProp.Add(GetNavProp[i].ToString(), GetListPro[i]);
        //    }
        //    return NavPropWithProp;
        //}


        //public object[] GetPropertiesFromNavProp(List<T>entity, Type decision) 
        //{


        //string[] Getnav = Enum.GetNames(decision);

        //    List<object> Prop = new List<object>();
        //    object[] TabNav = new object[Getnav.ToArray().Length+1];


        //    var j = 0;
        //    foreach (var prop in Getnav)
        //    {


        //        for (int i = 0; i < entity.Count; i++)
        //        {
        //            object col = entity[i].LoadProperties(prop, _repository);

        //            Prop.Add(col);
        //        }

        //        TabNav[j] = Prop.ToArray();
        //        j++;
        //        Prop.Clear();
        //    }
        //    TabNav[j] = entity;

        //    return TabNav;
        //}

        //---------------------------------------------

        public List<string> GetAllFilteredNavsPropertiesListEntity(List<T> entity, Type decision)
        {
            List<string> Getnav = Enum.GetNames(decision).ToList();
            string GetEntityName = entity.First().GetType().Name;
            Getnav.Add(GetEntityName);
            return Getnav;
        }
        public List<string> GetAllFilteredNavsPropertiesSingleEntity(T entity, Type decision)
        {
            List<string> Getnav = Enum.GetNames(decision).ToList();
            string GetEntityName = entity.GetType().Name;
            Getnav.Add(GetEntityName);
            return Getnav;
        }
        public List<string> GetAllNavsPropertiesListEntity(List<T> entity)
        {
            List<string> Getnav = GetNaviProps(typeof(T)).ToList();
            string GetEntityName = entity.First().GetType().Name;
            Getnav.Add(GetEntityName);
            return Getnav;
        }
        public List<string> GetAllNavsPropertiesSingleEntity(T entity)
        {
            List<string> Getnav = GetNaviProps(typeof(T)).ToList();
            string GetEntityName = entity.GetType().Name;
            Getnav.Add(GetEntityName);
            return Getnav;
        }

        public Dictionary<string, object> GetFilteredNavProp(List<T> entity, Type decision)
        {
            Dictionary<string, object> NavPropWithProp = new Dictionary<string, object>();
            string[] GetNavProp = GetAllFilteredNavsPropertiesListEntity(entity, decision).ToArray();
            object[] GetListPro = GetAllFilteredPropertiesFromNavProp(entity, decision).ToArray();

            for (var i = 0; i < GetNavProp.ToArray().Length; i++)
            {

                NavPropWithProp.Add(GetNavProp[i].ToString(), GetListPro[i]);
            }
            return NavPropWithProp;
        }
        public Dictionary<string, object> GetAllNavProp(T entity)
        {
            Dictionary<string, object> NavPropWithProp = new Dictionary<string, object>();
            string[] GetNavProp = GetAllNavsPropertiesSingleEntity(entity).ToArray();
            object[] GetListPro = GetAllPropertiesFromNavProp(entity).ToArray();

            for (var i = 0; i < GetNavProp.ToArray().Length; i++)
            {

                NavPropWithProp.Add(GetNavProp[i].ToString(), GetListPro[i]);
            }
            return NavPropWithProp;
        }

        public Dictionary<string, object> GetFilteredNavProp(T entity, Type decision)
        {
            Dictionary<string, object> NavPropWithProp = new Dictionary<string, object>();
            string[] GetNavProp = GetAllFilteredNavsPropertiesSingleEntity(entity, decision).ToArray();
            object[] GetListPro = GetAllFilteredPropertiesFromNavProp(entity, decision).ToArray();

            for (var i = 0; i < GetNavProp.ToArray().Length; i++)
            {

                NavPropWithProp.Add(GetNavProp[i].ToString(), (object)GetListPro[i]);
            }
            return NavPropWithProp;
        }
        public Dictionary<string, object> GetAllNavProp(List<T> entity)
        {
            Dictionary<string, object> NavPropWithProp = new Dictionary<string, object>();
            string[] GetNavProp = GetAllNavsPropertiesListEntity(entity).ToArray();
            object[] GetListPro = GetAllPropertiesFromNavProp(entity).ToArray();

            for (var i = 0; i < GetNavProp.ToArray().Length; i++)
            {

                NavPropWithProp.Add(GetNavProp[i].ToString(), GetListPro[i]);
            }
            return NavPropWithProp;
        }


        public object[] GetAllFilteredPropertiesFromNavProp(List<T> entity, Type decision)
        {
            string[] Getnav = Enum.GetNames(decision);

            List<object> Prop = new List<object>();
            object[] TabNav = new object[Getnav.ToArray().Length + 1];


            var j = 0;
            foreach (var prop in Getnav)
            {


                for (int i = 0; i < entity.Count; i++)
                {
                    object col = entity[i].LoadProperties(prop, _repository);

                    Prop.Add(col);
                }

                TabNav[j] = Prop.ToArray();
                j++;
                Prop.Clear();
            }
            TabNav[j] = entity;

            return TabNav;
        }
        public object[] GetAllFilteredPropertiesFromNavProp(T entity, Type decision)
        {


            string[] Getnav = Enum.GetNames(decision);

            List<object> Prop = new List<object>();
            object[] TabNav = new object[Getnav.ToArray().Length + 1];


            var j = 0;
            foreach (var prop in Getnav)
            {



                object col = entity.LoadProperties(prop, _repository);

                Prop.Add(col);


                TabNav[j] = Prop.ToArray();
                j++;
                Prop.Clear();
            }
            TabNav[j] = entity;

            return TabNav;
        }
        public object[] GetAllPropertiesFromNavProp(List<T> entity)
        {
            string[] Getnav = GetNaviProps(typeof(T)).ToArray();

            List<object> Prop = new List<object>();
            object[] TabNav = new object[Getnav.ToArray().Length + 1];


            var j = 0;
            foreach (var prop in Getnav)
            {



                object col = entity.First().LoadProperties(prop, _repository);

                Prop.Add(col);


                TabNav[j] = Prop.ToArray();
                j++;
                Prop.Clear();
            }
            TabNav[j] = entity;

            return TabNav;
        }
        public object[] GetAllPropertiesFromNavProp(T entity)
        {
            string[] Getnav = GetNaviProps(typeof(T)).ToArray();

            List<object> Prop = new List<object>();
            object[] TabNav = new object[Getnav.ToArray().Length + 1];


            var j = 0;
            foreach (var prop in Getnav)
            {



                object col = entity.LoadProperties(prop, _repository);

                Prop.Add(col);


                TabNav[j] = Prop.ToArray();
                j++;
                Prop.Clear();
            }
            List<T> TransformToList = new();
            TransformToList.Add(entity);
            TabNav[j] = TransformToList;

            return TabNav;
        }

        private static string[] GetNaviProps(Type entityType)//eg typeof(Employee)
        {
            return entityType.GetProperties()
                             .Where(p => (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && p.PropertyType != typeof(string)) || p.PropertyType.Namespace == entityType.Namespace)
                             .Select(p => p.Name)
                             .ToArray();
        }

    }

}
