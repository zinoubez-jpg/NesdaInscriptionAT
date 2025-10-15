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

    public class Inscription_CirconscriptionBusinessService : SuperBusinessService<Inscription_Circonscription, int>
    {
        //private readonly IStringLocalizer<BusinessMessage> _localizer;

        public Inscription_CirconscriptionBusinessService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {
        }

        public override Expression<Func<Inscription_Circonscription, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Circonscription, object>> loadAnnexe = x => x.Inscription_Annexe;
            Expression<Func<Inscription_Circonscription, object>> loadCommune = x => x.Inscription_Commune;

            return new Expression<Func<Inscription_Circonscription, object>>[] { loadAnnexe, loadCommune };
        }

        public Inscription_Circonscription GetByCommuneId(int Commune_Id)
        {

            Inscription_Circonscription result = GetSingle(x => x.Inscription_CommuneId == Commune_Id, GetDefaultLoadProperties());

            return result;
        }
        public List<Inscription_Circonscription> GetByAnnexeId(int Annexe_Id)
        {

            List<Inscription_Circonscription> result = GetAllFiltered(x => x.Inscription_AnnexeId == Annexe_Id, GetDefaultLoadProperties());

            return result;
        }
    }
}
