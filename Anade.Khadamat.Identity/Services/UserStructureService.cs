using Anade.Business.Core;
using Anade.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anade.Khadamat.Identity
{
    public class UserStructureService : GenericBusinessService<UserStructure, int>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserStructureService(IUnitOfWork<IdentityContext> unitOfWork, UserManager<User> userManager, RoleManager<Role> roleManager) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Expression<Func<UserStructure, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<UserStructure, object>> loadUser = x => x.User;
            Expression<Func<UserStructure, object>> loadStructure = x => x.Structure;


            return new Expression<Func<UserStructure, object>>[] { loadUser, loadStructure };
        }
    }
}
