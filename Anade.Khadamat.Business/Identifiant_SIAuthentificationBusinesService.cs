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
    public class Identifiant_SIAuthentificationBusinesService : GenericBusinessService<Authentification_Identifiant_SI, int>
    {
        public Identifiant_SIAuthentificationBusinesService(IUnitOfWork<AuthentificationDbContext> unitOfWork) : base(unitOfWork)
        {

        }

    }
}
