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
    public class CommuneBusinessService : ReferentielBusinessService<Commune, int>
    {
        public CommuneBusinessService(IUnitOfWork unitOfWork ) : base(unitOfWork)
        {
        }

        public override Expression<Func<Commune, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Commune, object>> loadWilaya = x => x.Wilaya;
            return new Expression<Func<Commune, object>>[] { loadWilaya };
        }
    }

}
