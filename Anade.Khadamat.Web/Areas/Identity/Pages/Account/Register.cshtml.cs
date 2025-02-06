using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Areas.Identity.Pages.Account
{
    //[Authorize(Roles = "Admin,AdminAgence")]
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IdentityContext _context;
        private readonly TypeUserServices _typeUserService;
        private readonly StructureService _structureService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly UserRoleService _userRoleService;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserStructureService _userStructureService;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly NiveauService _niveauService;

        public RegisterModel(
            UserService userService,
            IdentityContext context,
            TypeUserServices typeUserService,
            StructureService structureService,
            UserManager<User> userManager,
            UserRoleService userRoleService,
            RoleManager<Role> roleManager,
            UserStructureService userStructureService,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            NiveauService niveauService)
        {
            _context = context;
            _typeUserService = typeUserService;
            _structureService = structureService;
            _userManager = userManager;
            _userRoleService = userRoleService;
            _userStructureService = userStructureService;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _userService = userService;
            _niveauService = niveauService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            public string Nom { get; set; }
            [Required]
            public string Prenom { get; set; }

            public DateTime DateCreation { get; set; }
            public int? OrganismeId { get; set; }
            [Required]
            public int TypeUserId { get; set; }
            [Required]
            public int StructureId { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var user = await _userService.GetUserEagerLoadedAsync(User);


            var structure = await _userService.GetStructureFromUserAsync(user.Id);


            if (structure.Designation == "DG")
            {
                var listStructure = _structureService.GetAllFiltered(x => x.Niveau.Code == "1" || x.Niveau.Code == "2", _structureService.GetDefaultLoadProperties());
                ViewData["StructureID"] = new SelectList(listStructure, "Id", "NameStructure");
                ViewData["TypeUserID"] = new SelectList(_typeUserService.GetAll(), "Id", "Designation");
            }
            else
            {
                var listStructure = _structureService.GetAllFiltered(x => x.CodeStructure.StartsWith(structure.CodeStructure));
                ViewData["StructureID"] = new SelectList(listStructure, "Id", "NameStructure");
                ViewData["TypeUserID"] = new SelectList(_typeUserService.GetAllFiltered(x => x.Code == "0001"), "Id", "Designation");
            }

            //ViewData["StructureID"] = new SelectList(_structureService.GetAll(), "Id", "Designation");
            //ViewData["TypeUserID"] = new SelectList(_typeUserService.GetAll(), "Id", "Designation");


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/AccountGestion/Index");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                string cleanedPrenom = Input.Prenom.Replace(" ", "_");
                string cleanedNom = Input.Nom.Replace(" ", "_");
                var userNew = new User
                {
                    UserName = cleanedNom + "." + cleanedPrenom,
                    Email = cleanedPrenom.ToLower() + "." + cleanedNom.ToLower() + "@ansej.lan",
                    Nom = Input.Nom,
                    Prenom = Input.Prenom,
                    DateCreation = DateTime.Now,
                    TypeUserId = Input.TypeUserId,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(userNew, Input.Password);
                UserStructure userStructure = new UserStructure()
                {
                    Actuelle = true,
                    StructureId = Input.StructureId,
                    UserId = userNew.Id,
                    DateAffectation = DateTime.Now


                };

                _userStructureService.Add(userStructure);

                var roles = await _userService.GetAllRolesAsync();
                var structureNew = await _userService.GetStructureFromUserAsync(userNew.Id);

                if (userNew.TypeUserId == 1)
                {

                    if (structureNew.Niveau.Code == "2")
                    {
                        var role = roles.Find(r => r.Name == "Formateur");

                        UserRole userRole = new UserRole()
                        {
                            UserId = userNew.Id,
                            RoleId = role.Id

                        };
                        _userRoleService.Add(userRole);

                    }
                    else if (structureNew.Niveau.Code == "3")
                    {
                        var role = roles.Find(r => r.Name == "Anade");
                        UserRole userRole = new UserRole()
                        {
                            UserId = userNew.Id,
                            RoleId = role.Id

                        };

                        _userRoleService.Add(userRole);

                    }

                    else
                    {
                        var role = roles.Find(r => r.Name == "DG");
                        UserRole userRole = new UserRole()
                        {
                            UserId = userNew.Id,
                            RoleId = role.Id

                        };

                        _userRoleService.Add(userRole);

                    }



                }




                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(userNew);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userNew.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { userName = Input.Prenom + "." + Input.Nom, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(userNew, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
