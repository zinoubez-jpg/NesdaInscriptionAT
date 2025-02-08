using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class DecisionBusinessService : GenericBusinessService<Decision, int>
    {


        public DecisionBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           

        }

        public override Expression<Func<Decision, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Decision, object>> loadTDecision = x => x.TypeDecision;
            Expression<Func<Decision, object>> loadInscription = x => x.Inscription;
            return new Expression<Func<Decision, object>>[] { loadTDecision, loadInscription};
        }
    }
}

