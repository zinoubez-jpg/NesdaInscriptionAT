using Anade.Data.Abstractions;
using Anade.Domain.Core;
using Anade.Khadamat.Domain;
using Anade.Business.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anade.Khadamat.Business
{
    public class ReferentielBusinessService<T, TKey> : GenericBusinessService<T, TKey> where T : Referentiel, IEntity<TKey>
    {
        public ReferentielBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        protected override void OnAdding(T entity)
        {
            //Indication: Use the repository.Count(predicate) method
            if (_repository.Count(x => x.Code == entity.Code || x.DesignationFr == entity.DesignationFr || x.DesignationAr == entity.DesignationAr) > 0)
                throw new BusinessException("Un référentiel ayant le même code ou la même designation existe déja!");

            base.OnAdding(entity);
        }

        protected override void OnUpdating(T entity)
        {            
            //Indication: Use the repository.Count(predicate) method
            if (_repository.Count(x => x.Id != entity.Id && (x.Code == entity.Code || x.DesignationFr == entity.DesignationFr || x.DesignationAr == entity.DesignationAr)) > 0)
                throw new BusinessException("Un référentiel ayant le même code ou la même designation existe déja!");

            base.OnUpdating(entity);
        }

    }
}
