using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class SmsSentBusinessService : GenericBusinessService<SmsSent, int>
    {
        public SmsSentBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Expression<Func<SmsSent, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<SmsSent, object>> loadType = x => x.TypeSms;
            Expression<Func<SmsSent, object>> loadStatut = x => x.StatutEnvoiSms;
            Expression<Func<SmsSent, object>> loadInscription = x => x.Inscription;
            return new Expression<Func<SmsSent, object>>[] { loadType, loadStatut, loadInscription };
        }
    }
}
