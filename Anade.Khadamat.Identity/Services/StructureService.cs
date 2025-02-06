using Anade.Business.Core;
using Anade.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Identity.Services
{
   public class StructureService : GenericBusinessService<Structure, int>
    {
        public StructureService(IUnitOfWork<IdentityContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Structure, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<Structure, object>> loadNiveau = x => x.Niveau;


            return new Expression<Func<Structure, object>>[] { loadNiveau };
        }
    }
}
