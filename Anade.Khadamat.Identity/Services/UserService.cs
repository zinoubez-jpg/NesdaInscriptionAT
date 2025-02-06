using Anade.Business.Core;
using Anade.Data.Abstractions;
using Anade.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Anade.Khadamat.Identity
{
    public class UserService : GenericBusinessService<User, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(IUnitOfWork<IdentityContext> unitOfWork, UserManager<User> userManager, RoleManager<Role> roleManager) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserStructure>> GetProjetsByAccompagnateurAsync(ClaimsPrincipal user)
        {
            var accompagnateur = await _userManager.GetUserAsync(user);
            var structure = await GetUserStructureAsync(accompagnateur.Id);
            return _unitOfWork.GetRepository<UserStructure, int>().GetAllFiltered(x => x.StructureId == structure.Id && x.User.TypeUserId == Id.Projet, x => x.User.TypeUser);
        }
        public async Task<Structure> GetStructureFromUserAsync(string userId)
        {
            return await GetUserStructureAsync(userId);
        }
        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await Task.Run(() => _roleManager.Roles.ToList());
        }






        #region All GetMethods around "User" conscern

        public async Task<User> GetUserEagerLoadedAsync(ClaimsPrincipal userClaims)
        {
            var user = await _userManager.GetUserAsync(userClaims);
            return await Task.Run(() => _unitOfWork.GetRepository<User, string>().GetSingle(x => x.Id == user.Id, x => x.Structures));
        }
        public async Task<User> GetUserEagerLoadedAsync(string userId)
        {
            return await Task.Run(() => _unitOfWork.GetRepository<User, string>().GetSingle(x => x.Id == userId, x => x.Structures));
        }





        public async Task<List<UserStructure>> GetProjetsByStructureAsync(int structureId)
          => await Task.Run(() => _unitOfWork.GetRepository<UserStructure, int>().GetAllFiltered(x => x.Structure.Id == structureId && x.User.TypeUserId == Id.Projet, x => x.User.TypeUser));
        public async Task<List<UserStructure>> GetUsersByStructuresAsync(int structureId)       
          => await Task.Run(() => _unitOfWork.GetRepository<UserStructure, int>().GetAllFiltered(x => x.Structure.Id == structureId && x.Actuelle == true));  
        public async Task<Structure> GetUserStructureAsync(string userId)
          => await Task.Run(() => _unitOfWork.GetRepository<UserStructure, int>().GetSingle(x => x.UserId == userId && x.Actuelle == true, x => x.Structure,x=>x.Structure.Niveau).Structure);    
        public async Task<Structure> GetStructureByCodeAsync(string code, int niveauId)
          => await Task.Run(() => _unitOfWork.GetRepository<Structure, int>().GetSingle(x => x.CodeStructure == code && x.Niveau.Id == niveauId, x => x.Niveau));    
        public async Task<Structure> GetStructureAsync(int structure)
          => await Task.Run(() => _unitOfWork.GetRepository<Structure, int>().GetSingle(x => x.Id == structure));    
        public async Task<TypeUser> GetTypeUserAsync(string userId)
          => await Task.Run(() => _unitOfWork.GetRepository<User, string>().GetSingle(x => x.Id == userId, x => x.TypeUser).TypeUser); 
        public async Task<List<UserRole>> GetUsersByRoleAsync(string roleId)
          => await Task.Run(() => _unitOfWork.GetRepository<UserRole, int>().GetAllFiltered(x => x.RoleId == roleId));
        public async Task<List<UserRole>> GetRoleFromUserAsync(string userId)
          => await Task.Run(() => _unitOfWork.GetRepository<UserRole, int>().GetAllFiltered(x => x.UserId == userId));
        public async Task<IEnumerable<User>> GetUsersFilteredAsync(Expression<Func<User, bool>> predicate)
          => await Task.Run(() => _repository.GetAllFiltered(predicate));


        #endregion

        public override Expression<Func<User, object>>[] GetDefaultLoadProperties()
        {

            Expression<Func<User, object>> loadTypeUser = x => x.TypeUser;


            return new Expression<Func<User, object>>[] { loadTypeUser };
        }

    }
}
