using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Inscription;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Anade.Khadamat.Business
{
    public class AssociesInscriptionBusinesService : GenericBusinessService<Inscription_Associes, int>
    {
        public AssociesInscriptionBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Inscription_Associes, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Associes, object>> loadCategorie = x => x.Inscription_Categorie;

            return new Expression<Func<Inscription_Associes, object>>[] { loadCategorie };
        }
    }
}
