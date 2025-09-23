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
    public class Inscription_AntenneBusinesService : GenericBusinessService<Inscription_Antenne, int>
    {
        public Inscription_AntenneBusinesService(IUnitOfWork<InscriptionDbContext> unitOfWork) : base(unitOfWork)
        {

        }
    }
}
