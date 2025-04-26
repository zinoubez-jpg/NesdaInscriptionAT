using Anade.Business.Core;
using Anade.Domain.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Data;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Anade.Khadamat.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Anade.Khadamat.Web.Controllers
{
    [Route("[controller]/[action]")]
  
    public class InscriptionController : Controller
    {

        private readonly ReferentielBusinessService<Commune, int> _communeBusinessService;
        private readonly ReferentielBusinessService<Categorie, int> _categorieBusinessService;
        private readonly ReferentielBusinessService<Antenne, int> _antenneBusinessService;

        private readonly ReferentielBusinessService<Wilaya, int> _wilayaBusinessService;
        private readonly ReferentielBusinessService<Genre, int> _genreBusinessService;
        private readonly ReferentielBusinessService<SituationFamiliale, int> _situationFamilialeBusinessService;
        private readonly InscriptionBusinessService _inscriptionBusinessService;
        private readonly ReferentielBusinessService<NiveauInstruction, int> _niveauInstructionBusinessService;
        private readonly KhadamtDbContext _context;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly ReferentielBusinessService<TypeInscription, int> _typeInscriptionBusinessService;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly CirconscriptionBusinessService _circonscriptionBusinessService;
        private readonly ReferentielBusinessService<Activite, int> _activiteBusinessService;




        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly UserStructureService _userStructureService;
        private readonly TypeUserServices _typeUserServices;
        private readonly StructureService _structureService;
        private readonly ReferentielBusinessService<TypeFinancement, int> _typeFinnacementBusinessService;
        public InscriptionController(                           
                                      ReferentielBusinessService<Commune, int> communeBusinessService,
                                      ReferentielBusinessService<Wilaya, int> wilayaBusinessService,
                                      ReferentielBusinessService<Genre, int> genreBusinessService,
                                      ReferentielBusinessService<SituationFamiliale, int> situationFamilialeBusinessService,
                                      InscriptionBusinessService inscriptionBusinessService,
                                      ReferentielBusinessService<NiveauInstruction, int> niveauInstructionBusinessService,
                                      KhadamtDbContext context,
                                      IEmailVerificationService emailVerificationService,
                                      ReferentielBusinessService<TypeInscription, int> typeInscriptionBusinessService,
                                      IRecaptchaService recaptchaService,
                                      IHostingEnvironment hostingEnvironment,
                                      ReferentielBusinessService<Antenne, int> antenneBusinessService,

                                      UserManager<User> userManager,
                                      RoleManager<Role> roleManager,
                                      UserService userService,
                                      UserStructureService userStructureService,
                                      TypeUserServices typeUserServices,
                                      StructureService structureService,
                                      ReferentielBusinessService<Categorie, int> categorieBusinessService,
                                      CirconscriptionBusinessService circonscriptionBusinessService,
                                      ReferentielBusinessService<Activite, int> activiteBusinessService,
                                      ReferentielBusinessService<TypeFinancement, int> typeFinnacementBusinessService

                                   )
        {
            _communeBusinessService = communeBusinessService;
            _wilayaBusinessService = wilayaBusinessService;
            _genreBusinessService = genreBusinessService;
            _situationFamilialeBusinessService = situationFamilialeBusinessService;
            _inscriptionBusinessService = inscriptionBusinessService;
            _niveauInstructionBusinessService = niveauInstructionBusinessService;
            _context = context;
            _emailVerificationService = emailVerificationService;
            _typeInscriptionBusinessService = typeInscriptionBusinessService;
            _recaptchaService = recaptchaService;
            _hostingEnvironment = hostingEnvironment;
            _categorieBusinessService = categorieBusinessService;
            _circonscriptionBusinessService = circonscriptionBusinessService;
            _antenneBusinessService = antenneBusinessService;
            _activiteBusinessService = activiteBusinessService;
            _typeFinnacementBusinessService = typeFinnacementBusinessService;


            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
            _userStructureService = userStructureService;
            _typeUserServices = typeUserServices;
            _structureService = structureService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("{TypeInscriptionId}")]
        public virtual IActionResult CreateInscription(int typeInscriptionId)
        {
            if ( typeInscriptionId != 2)
            {
                return  Unauthorized();
            }

             ViewBag.TypeInsId = typeInscriptionId;

          

            InitializeListViewBags();

            return View();
        }
        // POST: Generic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{TypeInscriptionId}")]
        public virtual IActionResult CreateInscription(int typeInscriptionId, CreateInscriptionVM model)
         {
            if (typeInscriptionId != 1 && typeInscriptionId != 2)
            {
                return Unauthorized();
            }
            var isRecaptchaValid = _recaptchaService.ValidateRecaptchaAsync(model.RecaptchaResponse).Result ;
            if (!isRecaptchaValid)
            {
                ModelState.AddModelError("Recaptcha", "لم تتمكن من اجتياز اختبار الـ CAPTCHA. يرجى المحاولة مرة أخرى.");
                InitializeListViewBags();

                var bR = new BusinessResult();
                var msgR = new MessageResult()
                {
                    Message = "لم تتمكن من اجتياز اختبار الـ CAPTCHA. يرجى المحاولة مرة أخرى.",
                    MessageType = MessageType.Error,
                };

                bR.Messages.Add(msgR);
                TempData["Message"] = bR.ToBootstrapAlerts();

                return View(model);
            }

            if (ModelState.IsValid)
            {
                if ( _context.Inscriptions.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر");
                    InitializeListViewBags();
                    var bR = new BusinessResult();
                    var msgR =new MessageResult()
                    {
                        Message= "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر",
                        MessageType = MessageType.Error,
                    };
                    
                    bR.Messages.Add(msgR);
                    TempData["Message"] = bR.ToBootstrapAlerts();

                    return View(model);
                }


                var bResult = new BusinessResult(false);
                if (model.CopieCNIPdf.Length > 2048576)
                {

                    bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });

                    if (bResult.Succeeded)
                    {
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        return RedirectToAction(nameof(Index));
                    }

                }
                if (!model.CopieCNIPdf.ContentType.Equals("application/pdf"))
                {

                    bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult { Message = "يرجى تحميل ملف بصيغة PDF لاستكمال طلبك.", MessageType = MessageType.Error });


                    if (bResult.Succeeded)
                    {
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        return RedirectToAction(nameof(Index));
                    }

                }
                string copieCNIPdfPath = ProcessUploadedFile(model, model.CopieCNIPdf, "_CNI", "CNIFolder"); ;


                if (model.CopieDiplomeRCCFCAPdf.Length > 2048576)
                {

                    bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });

                    if (bResult.Succeeded)
                    {
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        return RedirectToAction(nameof(Index));
                    }

                }
                if (!model.CopieDiplomeRCCFCAPdf.ContentType.Equals("application/pdf"))
                {

                    bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult { Message = "يرجى تحميل ملف بصيغة PDF لاستكمال طلبك.", MessageType = MessageType.Error });


                    if (bResult.Succeeded)
                    {
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        return RedirectToAction(nameof(Index));
                    }

                }
                string copieDiplomeRCCFCAPdfPath = ProcessUploadedFile(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");
                var communeSS = _communeBusinessService.GetById(model.CommuneSeigeSocialId, _communeBusinessService.GetDefaultLoadProperties());
                var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                var antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);

                Inscription NewInscription = new Inscription
                {

                    TypeInscriptionId = typeInscriptionId,
                    CategorieId = model.CategorieId,
                    TypeFinancementId = model.TypeFinancementId,
                    DateInscription = DateTime.Now,
                    NIN = model.NIN,
                    NomAr = model.NomAr,
                    PrenomAr = model.PrenomAr,
                    NomFr = model.NomFr,
                    PrenomFr = model.PrenomFr,
                    DateNaissance = model.DateNaissance,
                    GenreId = model.GenreId,
                    SituationFamilialeId = model.SituationFamilialeId,
                    NomJeunneFille = model.NomJeunneFille,
                    NomJeunneFilleAr = model.NomJeunneFilleAr,
                    AdressAr = model.AdressAr,
                    WilayaResidenceId = model.WilayaResidenceId,
                    CommuneResidenceId = model.CommuneResidenceId,
                    WilayaNaissanceId = model.WilayaNaissanceId,
                    CommuneNaissanceId = model.CommuneNaissanceId,
                    WilayaSeigeSocialId = model.WilayaSeigeSocialId,
                    CommuneSeigeSocialId = model.CommuneSeigeSocialId,
                    NumTelephone = model.NumTelephone?.Replace(" ", ""),
                    Email = model.Email,
                    NiveauInstructionId = model.NiveauInstructionId,

                    EmailVerificationToken = Guid.NewGuid().ToString("N"),
                    EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
                    RecaptchaResponse = model.RecaptchaResponse,
                    CopieCNIPath = copieCNIPdfPath,
                    CopieDiplomeRCCFCAPath = copieDiplomeRCCFCAPdfPath,
                    AntenneId = antenne.Id,
                    ActiviteId = 1,
                    PasswordHashInit= model.Password


                };


                var businessResult = _inscriptionBusinessService.Add(NewInscription);


                if (businessResult.Succeeded)
                {
                    _emailVerificationService.SendVerificationEmailAsync(
                      NewInscription.Email,
                      NewInscription.EmailVerificationToken
                     );

                    //TempData["Message"] = businessResult.ToBootstrapAlerts();

                    return RedirectToAction(nameof(SuccesInscription));

                }
                TempData["Message"] = businessResult.ToBootstrapAlerts();
                InitializeListViewBags();

            }
            InitializeListViewBags();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Paramètres invalides");
            }

            var verified = await _emailVerificationService.VerifyEmailAsync(email, token);

            if (verified)
            {
                    var inscri = _inscriptionBusinessService.GetSingle(x => x.Email == email, _inscriptionBusinessService.GetDefaultLoadProperties());
                    var userNew = new User
                    {
                        UserName = inscri.Email,
                        Email = inscri.Email,
                        DateCreation = DateTime.Now,
                        TypeUserId = 2,
                        EmailConfirmed = true
                    };
                    var password = inscri.PasswordHashInit;
                    var result = await _userManager.CreateAsync(userNew, password);
                return View("EmailVerificationSuccess");
            }

            return View("EmailVerificationFailed");
        }

        public IActionResult SuccesInscription()
        {
          
            return View();
        }

       


        #region Helper Methods
        private void InitializeListViewBags()
        {
            var genres = _genreBusinessService.GetAll();
            var situationFamiliales = _situationFamilialeBusinessService.GetAll();
            var communes = _communeBusinessService.GetAll();
            var wilayas = _wilayaBusinessService.GetAll();
            var niveauInstructions = _niveauInstructionBusinessService.GetAll();
            var typeInscription = _typeInscriptionBusinessService.GetAll();
            var categories = _categorieBusinessService.GetAllFiltered(x=>x.Id!=1);
            var typeFinancements = _typeFinnacementBusinessService.GetAll();


            ViewBag.GenreId = new SelectList(genres, nameof(Genre.Id), nameof(Genre.DesignationAr));
            ViewBag.SituationFamilialeId = new SelectList(situationFamiliales, nameof(SituationFamiliale.Id), nameof(SituationFamiliale.DesignationAr));
            ViewBag.CommuneResidenceId = new SelectList(communes, nameof(Commune.Id), nameof(Commune.DesignationAr));
            ViewBag.WilayaResId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));
            ViewBag.NiveauInstructionId = new SelectList(niveauInstructions, nameof(NiveauInstruction.Id), nameof(NiveauInstruction.DesignationAr));
            ViewBag.TypeInscriptionId = new SelectList(typeInscription, nameof(TypeInscription.Id), nameof(TypeInscription.DesignationAr));
            ViewBag.CategorieId = new SelectList(categories, nameof(Categorie.Id), nameof(Categorie.DesignationAr));
            ViewBag.TypeFinancementId = new SelectList(typeFinancements, nameof(TypeFinancement.Id), nameof(TypeFinancement.DesignationAr));
        }


        [HttpGet]
        public JsonResult GetCommuneList(int WilayaId)
        {
            var communelist = new SelectList(_communeBusinessService.GetAll().Where(x => x.WilayaId == WilayaId), "Id", "DesignationAr");
            return Json(communelist);
        }


        private string ProcessUploadedFile(CreateInscriptionVM model, IFormFile file, string fileNameSuffix, string folderName)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
                uniqueFileName = $"{model.NIN}{fileNameSuffix}.pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }



        #endregion




    }
}
