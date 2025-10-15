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
    public class Inscription_CommuneBusinesService : GenericBusinessService<Inscription_Commune, int>
    {
        public Inscription_CommuneBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
        public override Expression<Func<Inscription_Commune, object>>[] GetDefaultLoadProperties()
        {
            Expression<Func<Inscription_Commune, object>> loadWilaya = x => x.Inscription_Wilaya;

            return new Expression<Func<Inscription_Commune, object>>[] { loadWilaya };
        }

        public Inscription_Commune GetByCode(string code)
        {
            Inscription_Commune commune = new();

            commune = GetSingle(x => x.Code == code);

            return commune;
        }
    }
}
