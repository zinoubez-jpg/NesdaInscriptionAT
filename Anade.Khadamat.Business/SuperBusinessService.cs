using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Domain.Core;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class SuperBusinessService<T, TKey> : GenericBusinessService<T, TKey> where T : class, IEntity<TKey>
    {
        //TODO: Create a private readonly field _localizer of type IStringLocalizer<BusinessMessage>
        //private readonly IStringLocalizer<BusinessMessage> _localizer;

        //TODO Add constructor parameter localizer to initialize the _localizer field.
        public SuperBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //_localizer = localizer;

        }

        protected override void OnAdding(T entity)
        {
            //Indication: Use the repository.Count(predicate) method

            base.OnAdding(entity);
        }

        protected override void OnUpdating(T entity)
        {

            base.OnUpdating(entity);
        }
        public override Expression<Func<T, object>>[] GetDefaultLoadProperties()
        {

            return Array.Empty<Expression<Func<T, object>>>();
        }


    }

}

