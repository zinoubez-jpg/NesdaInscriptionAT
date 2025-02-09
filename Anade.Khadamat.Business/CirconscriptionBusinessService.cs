using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Khadamat.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Anade.Khadamat.Business
{
    public class CirconscriptionBusinessService : GenericBusinessService<Circonscription, int>
    {


        public CirconscriptionBusinessService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
           

        }
    }
}

