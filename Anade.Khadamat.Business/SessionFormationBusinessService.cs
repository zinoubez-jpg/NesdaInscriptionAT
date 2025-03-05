using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class SessionFormationBusinessService : GenericBusinessService<SessionFormationDetails, int>
    {


        public SessionFormationBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           

        }

        public override Expression<Func<SessionFormationDetails, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<SessionFormationDetails, object>> loadFormation = x => x.Formation;
            Expression<Func<SessionFormationDetails, object>> loadInscription = x => x.Inscription;
            return new Expression<Func<SessionFormationDetails, object>>[] { loadFormation, loadInscription};
        }
    }
}

