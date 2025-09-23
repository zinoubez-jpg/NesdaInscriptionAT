using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Authentification;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Anade.Khadamat.Business
{
    public class CompteAuthentificationBusinesService : GenericBusinessService<Authentification_Compte, int>
    {
        public CompteAuthentificationBusinesService(IUnitOfWork<AuthentificationDbContext> unitOfWork) : base(unitOfWork)
        {

        }

        public override Expression<Func<Authentification_Compte, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Authentification_Compte, object>> loadIdentifiantSI = x => x.Authentification_Identifiant_SI;

            return new Expression<Func<Authentification_Compte, object>>[] { loadIdentifiantSI };
        }

        public Authentification_Compte GetByUserName(string useruame)
        {
            Authentification_Compte compte = new();

            compte = GetSingle(x => x.Pseudo == useruame,GetDefaultLoadProperties());

            return compte;
        }
    }
}
