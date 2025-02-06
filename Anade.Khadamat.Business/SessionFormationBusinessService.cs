using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class SessionFormationBusinessService : GenericBusinessService<SessionFormation, int>
    {


        public SessionFormationBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           

        }

        public override Expression<Func<SessionFormation, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<SessionFormation, object>> loadFormation = x => x.Formation;
            Expression<Func<SessionFormation, object>> loadInscription = x => x.Inscription;
            return new Expression<Func<SessionFormation, object>>[] { loadFormation, loadInscription};
        }
    }
}

