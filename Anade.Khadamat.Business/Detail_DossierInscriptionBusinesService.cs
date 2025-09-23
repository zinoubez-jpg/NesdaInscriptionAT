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
    public class Detail_DossierInscriptionBusinesService : GenericBusinessService<Inscription_Detail_Dossier, int>
    {
        public Detail_DossierInscriptionBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Inscription_Detail_Dossier, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Detail_Dossier, object>> loadCommune = x => x.Inscription_Commune;

            return new Expression<Func<Inscription_Detail_Dossier, object>>[] { loadCommune };
        }
    }
}
