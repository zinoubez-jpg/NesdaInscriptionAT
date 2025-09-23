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
    public class Inscription_AnnexeBusinesService : GenericBusinessService<Inscription_Annexe, int>
    {
        public Inscription_AnnexeBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Inscription_Annexe, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Annexe, object>> loadInscriptionAntenne = x => x.Inscription_Antenne;

            return new Expression<Func<Inscription_Annexe, object>>[] { loadInscriptionAntenne };
        }
        public Inscription_Annexe GetByCodeAnnexe(string CodeAnnexe)
        {

            Inscription_Annexe result = GetSingle(x => x.Code == CodeAnnexe, GetDefaultLoadProperties());

            return result;
        }
    }
}
