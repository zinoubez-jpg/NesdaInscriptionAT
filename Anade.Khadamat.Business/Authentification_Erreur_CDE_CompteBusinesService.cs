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
    public class Authentification_Erreur_CDE_CompteBusinesService : GenericBusinessService<Authentification_Erreur_CDE_Compte, int>
    {
        public Authentification_Erreur_CDE_CompteBusinesService(IUnitOfWork<AuthentificationDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
