using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Data_Suivi;
using Anade.Khadamat.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Anade.Khadamat.Business
{
    public class Suivi_FormationBusinesService : GenericBusinessService<Suivi_Formation, int>
    {
        public Suivi_FormationBusinesService(IUnitOfWork<SuiviDbContext> unitOfWork) : base(unitOfWork)
        {

        }

    }
}
