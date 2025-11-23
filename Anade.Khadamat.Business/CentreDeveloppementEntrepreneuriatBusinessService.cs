using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Business
{
    public class CentreDeveloppementEntrepreneuriatBusinessService : ReferentielBusinessService<CentreDeveloppementEntrepreneuriat, int>
    {
        public CentreDeveloppementEntrepreneuriatBusinessService(IUnitOfWork unitOfWork ) : base(unitOfWork)
        {
        }

        public override Expression<Func<CentreDeveloppementEntrepreneuriat, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<CentreDeveloppementEntrepreneuriat, object>> loadWilaya = x => x.Wilaya;
            return new Expression<Func<CentreDeveloppementEntrepreneuriat, object>>[] { loadWilaya };
        }
    }

}
