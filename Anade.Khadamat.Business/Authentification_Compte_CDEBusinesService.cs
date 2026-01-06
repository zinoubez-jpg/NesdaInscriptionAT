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
    public class Authentification_Compte_CDEBusinesService : GenericBusinessService<Authentification_Compte_CDE, int>
    {
        public Authentification_Compte_CDEBusinesService(IUnitOfWork<AuthentificationDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
