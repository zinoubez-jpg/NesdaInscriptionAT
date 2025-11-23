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
    public class Authentification_CDEBusinesService : GenericBusinessService<Authentification_CDE, int>
    {
        public Authentification_CDEBusinesService(IUnitOfWork<AuthentificationDbContext> unitOfWork) : base(unitOfWork)
        {

        }

        public Authentification_CDE GetByCode(string code)
        {
            Authentification_CDE cde = new();

            cde = GetSingle(x => x.Code_CDE == code, GetDefaultLoadProperties());

            return cde;
        }

        public Authentification_CDE GetByCodeNew(string codeNew)
        {
            Authentification_CDE cde = new();

            cde = GetSingle(x => x.Code_CDE_New == codeNew, GetDefaultLoadProperties());

            return cde;
        }
    }
}
