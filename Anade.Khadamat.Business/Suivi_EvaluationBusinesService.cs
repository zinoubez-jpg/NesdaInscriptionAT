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
    public class Suivi_EvaluationBusinesService : GenericBusinessService<Suivi_Evaluation, int>
    {
        public Suivi_EvaluationBusinesService(IUnitOfWork<SuiviDbContext> unitOfWork) : base(unitOfWork)
        {

        }

    }
}
