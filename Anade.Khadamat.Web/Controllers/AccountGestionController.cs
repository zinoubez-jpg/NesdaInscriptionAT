using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Anade.Khadamat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Authorize(Roles = "Admin,AdminAgence")]
    public class AccountGestionController : Controller
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly UserService _userService;
        private readonly UserStructureService _userStructureService;
        private readonly TypeUserServices _typeUserServices;
        private readonly StructureService _structureService;
        public AccountGestionController(UserManager<User> userManager, RoleManager<Role> roleManager, UserService userService, UserStructureService userStructureService, TypeUserServices typeUserServices, StructureService structureService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _userService = userService;
            _userStructureService = userStructureService;
            _typeUserServices = typeUserServices;
            _structureService = structureService;




        }

        public async Task<IActionResult> Index()
        {
            List<UserStructure> model = new List<UserStructure>();
            return View(model);
        }


        public async Task<IActionResult> ListUsers()
        {
            List<User> model = new List<User>();


            return View(model);
        }



        [HttpPost]
        public virtual async Task<IActionResult> UserDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = _userService.GetStructureFromUserAsync(user.Id).Result;

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                var result = _userService.GetAllFilteredPaged(x => x.UserName.Contains(search),

                                                                      orderBy, startRowIndex, maxRows, _userService.GetDefaultLoadProperties());


                return Json(new JQueryDataTableRetunedData<User> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
            else
            {
                var result = _userService.GetAllFilteredPaged(x => x.Structures.FirstOrDefault().Structure.CodeStructure.StartsWith(structure.CodeStructure)
                , orderBy, startRowIndex, maxRows, _userService.GetDefaultLoadProperties());



                return Json(new JQueryDataTableRetunedData<User> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
        }



        [HttpPost]
        public virtual async Task<IActionResult> USDataTable(DataTableAjaxModel model)
        {
            var user = await _userService.GetUserEagerLoadedAsync(User);
            var structure = _userService.GetStructureFromUserAsync(user.Id).Result;

            GetDataTableParameters(model, out string search, out string orderBy, out int startRowIndex, out int maxRows);

            if (!string.IsNullOrEmpty(search))
            {
                var result = _userStructureService.GetAllFilteredPaged(x => x.Structure.CodeStructure.StartsWith(structure.CodeStructure)
                                                                     && (x.Structure.Designation.Contains(search)
                                                                     || x.User.UserName.Contains(search)),
                                                                      orderBy, startRowIndex, maxRows, _userStructureService.GetDefaultLoadProperties());


                return Json(new JQueryDataTableRetunedData<UserStructure> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
            else
            {
                var result = _userStructureService.GetAllFilteredPaged(x => x.Structure.CodeStructure.StartsWith(structure.CodeStructure)
                                                                    ,
                                                                      orderBy, startRowIndex, maxRows, _userStructureService.GetDefaultLoadProperties());

                return Json(new JQueryDataTableRetunedData<UserStructure> { draw = model.draw, recordsFiltered = result.TotalCount, recordsTotal = result.TotalCount, data = result.Items });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // unicite du nom de role 
                Role identityRole = new Role
                {
                    Name = model.RoleName
                };


                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Récupérer tous les utilisateurs
            foreach (var user in userManager.Users.ToList())
            {

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;


                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }





        protected static void GetDataTableParameters(DataTableAjaxModel model, out string search, out string orderBy, out int startRowIndex, out int maxRows)
        {
            maxRows = model.length;
            startRowIndex = model.start;
            string sortBy = "", sortDir = "";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
                orderBy = sortBy + " " + sortDir;
            }
            else
            {
                orderBy = "";
            }

            if (model.search.value != null)
            {
                search = model.search.value;
            }
            else
            {
                search = "";
            }

        }






        public async Task<IActionResult> Delete(int id)
        {

            var userStructure = _userStructureService.GetById(id, _userStructureService.GetDefaultLoadProperties());


            if (userStructure == null)
            {
                return NotFound();
            }

            return View(userStructure);
        }

        // POST: Report/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usStr = _userStructureService.GetById(id);

            var user = _userService.GetById(usStr.UserId);

            _userStructureService.Delete(usStr);
            _userService.Delete(user);




            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);
            ViewData["TypeUserID"] = new SelectList(_typeUserServices.GetAll(), "Id", "Designation");
            ViewData["StructureID"] = new SelectList(_structureService.GetAll(), "Id", "Designation");
            var userStructure = _userStructureService.GetSingle(x => x.UserId == user.Id);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email,
                TypeUserId = user.TypeUserId,
                Roles = userRoles,
                StructureId = userStructure.StructureId
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);



            var userStructure = _userStructureService.GetSingle(x => x.UserId == user.Id);
            userStructure.StructureId = model.StructureId;
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Nom = model.Nom;
                user.Prenom = model.Prenom;
                user.TypeUserId = model.TypeUserId;
                user.UserName = model.Nom + "." + model.Prenom;
                user.Email = model.Email;





                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    _userStructureService.Update(userStructure);
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUserStructure(int id)
        {
            var userStructure = _userStructureService.GetById(id, _userStructureService.GetDefaultLoadProperties());

            if (userStructure == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            ViewData["UserID"] = new SelectList(_userService.GetAll(), "Id", "FullName");
            ViewData["StructureID"] = new SelectList(_structureService.GetAll(), "Id", "Designation");

            var model = new UserStructure()
            {
                Id = userStructure.Id,
                UserId = userStructure.UserId,

                StructureId = userStructure.StructureId,


            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditUserStructure(UserStructure model)
        {
            var userStructure = _userStructureService.GetById(model.Id);

            if (userStructure == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                userStructure.UserId = model.UserId;
                userStructure.StructureId = model.StructureId;





                var result = _userStructureService.Update(userStructure);

                if (result.Succeeded)
                {

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Messages)
                {
                    ModelState.AddModelError("", error.Message);
                }

                return View(model);
            }
        }



    }

}
