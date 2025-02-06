using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Identity.Services
{
    public class TypeUserServices : GenericBusinessService<TypeUser, int> 
    {
        public TypeUserServices(IUnitOfWork<IdentityContext> unitOfWork) : base(unitOfWork)
        {

        }

        protected override void OnAdding(TypeUser entity)
        {
            //Indication: Use the repository.Count(predicate) method
            if (_repository.Count(x => x.Code == entity.Code || x.Designation == entity.Designation || x.DesignationAr == entity.DesignationAr) > 0)
                throw new BusinessException("Un référentiel ayant le même code ou la même designation existe déja!");

            base.OnAdding(entity);
        }

        protected override void OnUpdating(TypeUser entity)
        {
            //Indication: Use the repository.Count(predicate) method
            if (_repository.Count(x => x.Id != entity.Id && (x.Code == entity.Code || x.Designation == entity.Designation || x.DesignationAr == entity.DesignationAr)) > 0)
                throw new BusinessException("Un référentiel ayant le même code ou la même designation existe déja!");

            base.OnUpdating(entity);
        }

    }
}
