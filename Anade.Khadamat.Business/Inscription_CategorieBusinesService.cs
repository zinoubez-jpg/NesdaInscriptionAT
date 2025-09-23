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
    public class Inscription_CategorieBusinesService : GenericBusinessService<Inscription_Categorie, int>
    {
        public Inscription_CategorieBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Inscription_Categorie, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Categorie, object>> loadInscriptionPhase = x => x.Inscription_Phase;

            return new Expression<Func<Inscription_Categorie, object>>[] { loadInscriptionPhase };
        }
    }
}
