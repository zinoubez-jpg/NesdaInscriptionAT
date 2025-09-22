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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
        private readonly ReferentielBusinessService<Agence, int> _agenceBusinessService;
        private readonly ReferentielBusinessService<TypeCentreDeveloppementEntrepreneuriat, int> _typeCentreDeveloppementEntrepreneuriat;
        private readonly ReferentielBusinessService<CentreDeveloppementEntrepreneuriat, int> _centreDeveloppementEntrepreneuriatBusinessService;
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
                                      ReferentielBusinessService<TypeFinancement, int> typeFinnacementBusinessService,
                                      ReferentielBusinessService<Agence, int> agenceBusinessService,
                                      ReferentielBusinessService<TypeCentreDeveloppementEntrepreneuriat, int> typeCentreDeveloppementEntrepreneuriat,
                                      ReferentielBusinessService<CentreDeveloppementEntrepreneuriat, int> centreDeveloppementEntrepreneuriatBusinessService
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
            _agenceBusinessService = agenceBusinessService;
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
            _userStructureService = userStructureService;
            _typeUserServices = typeUserServices;
            _structureService = structureService;
            _typeCentreDeveloppementEntrepreneuriat = typeCentreDeveloppementEntrepreneuriat;
            _centreDeveloppementEntrepreneuriatBusinessService = centreDeveloppementEntrepreneuriatBusinessService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("{typeInscriptionId}")]
        public virtual IActionResult CreateInscription(int typeInscriptionId)
        {
            //if (typeInscriptionId != 2)
            //{
            //    return Unauthorized();
            //}
            ViewBag.TypeInsId = typeInscriptionId;
            InitializeListViewBags();
            return View();
        }
        // POST: Generic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{typeInscriptionId}")]
        public virtual async Task<IActionResult> CreateInscription(int typeInscriptionId, CreateInscriptionVM model)
        {
            if (typeInscriptionId != 2)
            {
                return Unauthorized();
            }
            if (model.CategorieId == 1)
            {

                var bResult = new BusinessResult();
                bResult.Messages.Add(new MessageResult { Message = "يرجى إختيار نوع الفئة بشكل صحيح", MessageType = MessageType.Error });
                TempData["Message"] = bResult.ToBootstrapAlerts();
                InitializeListViewBags();
                return View(model);
            }

            var isRecaptchaValid = _recaptchaService.ValidateRecaptchaAsync(model.RecaptchaResponse).Result;
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
                String apiResult = "";
                model.CommuneNaissance = _communeBusinessService.GetById(model.CommuneNaissanceId);
                model.CentreDeveloppementEntrepreneuriatId = 1;

                // Not nesda prometeur 
                if (model.CategorieId != 2)
                {
                    apiResult = await CheckPromoteurExistenceAsync(model);
                    if (apiResult != "\"false\"")
                    {

                        var bResult = new BusinessResult();
                        bResult.Messages.Add(new MessageResult
                        {
                            Message = apiResult, // TODO: to replace later with arabic msg
                            MessageType = MessageType.Error
                        });
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }
                }

                // Promoteur nesda (category ==2)
                else
                {
                    apiResult = await CheckExtensionEligibilityAsync(model);
                    bool isFourDigitCode = System.Text.RegularExpressions.Regex.IsMatch(apiResult, @"^\d{4}$");
                    if (!isFourDigitCode)
                    {

                        var bResult = new BusinessResult();
                        bResult.Messages.Add(new MessageResult
                        {
                            Message = apiResult,
                            MessageType = MessageType.Error
                        });
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                }

                // hckeing email
                if (_context.Inscriptions.Any(u => u.Email == model.Email && u.IsEmailVerified))
                {
                    ModelState.AddModelError("Email", "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر");
                    InitializeListViewBags();
                    var bR = new BusinessResult();
                    var msgR = new MessageResult()
                    {
                        Message = "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر",
                        MessageType = MessageType.Error,
                    };

                    bR.Messages.Add(msgR);
                    TempData["Message"] = bR.ToBootstrapAlerts();

                    return View(model);
                }


                if (model.CopieCNIPdf.Length > 2048576)
                {

                    var bResult = new BusinessResult();
                    bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });

                    TempData["Message"] = bResult.ToBootstrapAlerts();

                    InitializeListViewBags();

                    return View(model);


                }
                if (!IsValidPdfFile(model.CopieCNIPdf))
                {
                    var bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult
                    {
                        Message = "يرجى تحميل ملف بصيغة PDF صالح لاستكمال طلبك.",
                        MessageType = MessageType.Error
                    });

                    TempData["Message"] = bResult.ToBootstrapAlerts();
                    InitializeListViewBags();
                    return View(model);
                }
                string copieCNIPdfPath = ProcessUploadedFile(model, model.CopieCNIPdf, "_CNI", "CNIFolder"); ;


                if (model.CopieDiplomeRCCFCAPdf.Length > 2048576)
                {

                    var bResult = new BusinessResult();
                    bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });
                    TempData["Message"] = bResult.ToBootstrapAlerts();
                    InitializeListViewBags();
                    return View(model);
                }

                if (!IsValidPdfFile(model.CopieDiplomeRCCFCAPdf))
                {
                    var bResult = new BusinessResult(true);
                    bResult.Messages.Add(new MessageResult
                    {
                        Message = "يرجى تحميل ملف بصيغة PDF صالح لاستكمال طلبك.",
                        MessageType = MessageType.Error
                    });

                    TempData["Message"] = bResult.ToBootstrapAlerts();
                    InitializeListViewBags();
                    return View(model);
                }


                string copieDiplomeRCCFCAPdfPath = ProcessUploadedFile(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");
                var communeSS = _communeBusinessService.GetById(model.CommuneSeigeSocialId, _communeBusinessService.GetDefaultLoadProperties());

                Antenne antenne;
                Agence agence;

                if (model.CategorieId != 2)
                {
                    var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                    antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
                    agence = _agenceBusinessService.GetById(antenne.AgenceId);

                }
                else
                {
                    antenne = _antenneBusinessService.GetSingle(x => x.Code == apiResult);
                    agence = _agenceBusinessService.GetById(antenne.AgenceId);
                }


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
                    Anntenne = antenne.Code,
                    Agence = agence.Code,

                    ActiviteId = 1,
                    PasswordHashInit = model.Password,

                    CentreDeveloppementEntrepreneuriatId = model.CentreDeveloppementEntrepreneuriatId
            };


                var businessResult = _inscriptionBusinessService.Add(NewInscription);


                if (businessResult.Succeeded)
                {
                    _emailVerificationService.SendVerificationEmailAsync(
                      NewInscription.Email,
                      NewInscription.EmailVerificationToken
                     );



                    return RedirectToAction(nameof(SuccesInscription));

                }
                TempData["Message"] = businessResult.ToBootstrapAlerts();
                InitializeListViewBags();
                return View(model);

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



            if (_userService.Count(u => u.Email == email) > 0)
            {
                return View("EmailVerificationFailedExistEmail");

            }

            var verified = await _emailVerificationService.VerifyEmailAsync(email, token);

            if (!verified)
            {
                return View("EmailVerificationFailed");
            }

            var inscri = _inscriptionBusinessService.GetSingle(
                x => x.Email == email && x.EmailVerificationToken == null,
                _inscriptionBusinessService.GetDefaultLoadProperties());

            if (inscri == null)
            {
                return View("EmailVerificationFailed");
            }

            var userNew = new User
            {
                UserName = inscri.Email,
                Email = inscri.Email,
                DateCreation = DateTime.Now,
                TypeUserId = 2,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userNew, inscri.PasswordHashInit);

            if (result.Succeeded)
            {

                return View("EmailVerificationSuccess");
            }

            return View("EmailVerificationFailed");
        }

        public IActionResult SuccesInscription()
        {

            return View();
        }

        //[HttpGet]
        //public IActionResult CheckEmailExists(string email)
        //{
        //    bool emailExists = _context.Inscriptions.Any(u => u.Email == email && u.IsEmailVerified); 
        //    return Json(new { exists = emailExists });
        //}
        //[HttpGet]
        //public IActionResult CheckNINExists(string nin)
        //{
        //    bool ninExists = _context.Inscriptions.Any(u => u.NIN == nin && u.Validee == true && u.Annulee == false); 
        //    return Json(new { exists = ninExists });
        //}

        #region Helper Methods
        private void InitializeListViewBags()
        {
            var genres = _genreBusinessService.GetAll();
            var situationFamiliales = _situationFamilialeBusinessService.GetAll();
            var communes = _communeBusinessService.GetAll();
            var wilayas = _wilayaBusinessService.GetAll();
            var niveauInstructions = _niveauInstructionBusinessService.GetAll();
            var typeInscription = _typeInscriptionBusinessService.GetAll();
            var categories = _categorieBusinessService.GetAllFiltered(x => x.Id != 1);
            var typeFinancements = _typeFinnacementBusinessService.GetAll();
            var typesCentreDeveloppementEntrepreneuriat = _typeCentreDeveloppementEntrepreneuriat.GetAll();
            var centresDeveloppementEntrepreneuriat = _centreDeveloppementEntrepreneuriatBusinessService.GetAll();

            ViewBag.GenreId = new SelectList(genres, nameof(Genre.Id), nameof(Genre.DesignationAr));
            ViewBag.SituationFamilialeId = new SelectList(situationFamiliales, nameof(SituationFamiliale.Id), nameof(SituationFamiliale.DesignationAr));
            ViewBag.CommuneResidenceId = new SelectList(communes, nameof(Commune.Id), nameof(Commune.DesignationAr));
            ViewBag.WilayaResId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));
            ViewBag.NiveauInstructionId = new SelectList(niveauInstructions, nameof(NiveauInstruction.Id), nameof(NiveauInstruction.DesignationAr));
            ViewBag.TypeInscriptionId = new SelectList(typeInscription, nameof(TypeInscription.Id), nameof(TypeInscription.DesignationAr));
            ViewBag.CategorieId = new SelectList(categories, nameof(Categorie.Id), nameof(Categorie.DesignationAr));
            ViewBag.TypeFinancementId = new SelectList(typeFinancements, nameof(TypeFinancement.Id), nameof(TypeFinancement.DesignationAr));
            ViewBag.WilayaCentreDeveloppementEntrepreneuriatId = new SelectList(wilayas, nameof(TypeFinancement.Id), nameof(TypeFinancement.DesignationAr));
            ViewBag.WilayaCentreDeveloppementEntrepreneuriatId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));
            ViewBag.TypeCentreDeveloppementEntrepreneuriatId = new SelectList(typesCentreDeveloppementEntrepreneuriat, nameof(TypeCentreDeveloppementEntrepreneuriat.Id), nameof(TypeCentreDeveloppementEntrepreneuriat.DesignationAr));
            ViewBag.CentreDeveloppementEntrepreneuriatId = new SelectList(centresDeveloppementEntrepreneuriat, nameof(CentreDeveloppementEntrepreneuriat.Id), nameof(CentreDeveloppementEntrepreneuriat.DesignationAr));
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
                uniqueFileName = $"{model.NIN}{fileNameSuffix}{model.Id}.pdf";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        private bool IsValidPdfFile(IFormFile file)
        {
            if (file == null || file.Length < 5)
                return false;

            // Vérification de l'extension
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                return false;

            // Vérification du type MIME
            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return false;

            // Vérification du contenu : signature PDF ("%PDF-")
            try
            {
                using (var reader = new BinaryReader(file.OpenReadStream()))
                {
                    var headerBytes = reader.ReadBytes(5);
                    var header = System.Text.Encoding.ASCII.GetString(headerBytes);
                    return header.StartsWith("%PDF");
                }
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> CheckPromoteurExistenceAsync(CreateInscriptionVM model)
        {
            using var client = new HttpClient();
            var baseUrl = "https://promoteur.nesda.dz/Inscription/Verif_Existance_cr";

            var formattedDate = model.DateNaissance.ToString("dd/MM/yyyy");

            var encodedNom = HttpUtility.UrlEncode(model.NomFr);
            var encodedPrenom = HttpUtility.UrlEncode(model.PrenomFr);
            var encodedDate = HttpUtility.UrlEncode(formattedDate);
            var encodedNumActP = HttpUtility.UrlEncode(model.NIN.Substring(9, 5)); ;

            var url = $"{baseUrl}?NIN={model.NIN}&NOM_P={encodedNom}&PRENOM_P={encodedPrenom}&DDN_P={encodedDate}&NUM_ACT_P={encodedNumActP}&Commune_Nais={model.CommuneNaissance.Code}";

            try
            {
                var response = await client.GetStringAsync(url);
                return response;
            }
            catch (HttpRequestException ex)
            {
                return $"API Error";
            }
        }

        private async Task<string> CheckExtensionEligibilityAsync(CreateInscriptionVM model)
        {
            using var client = new HttpClient();
            var baseUrl = "https://promoteur.nesda.dz/Inscription/Verif_Existance_Ext";


            var formattedDate = model.DateNaissance.ToString("dd/MM/yyyy");
            var encodedNom = HttpUtility.UrlEncode(model.NomFr);
            var encodedPrenom = HttpUtility.UrlEncode(model.PrenomFr);
            var encodedDate = HttpUtility.UrlEncode(formattedDate);
            var encodedNumActP = HttpUtility.UrlEncode(model.NIN.Substring(9, 5));
            var url = $"{baseUrl}?NIN={model.NIN}&NOM_P={encodedNom}&PRENOM_P={encodedPrenom}&DDN_P={encodedDate}&NUM_ACT_P={encodedNumActP}&Commune_Nais={model.CommuneNaissance.Code}";

            try
            {
                var response = await client.GetStringAsync(url);
                return response.Trim('"');
            }
            catch (HttpRequestException ex)
            {
                return "Une erreur est survenue. Veuillez contacter l’administrateur.";
            }
        }

        [HttpGet]
        public JsonResult GetCentreDeveloppementEntrepreneuriatList(int WilayaId, int TypeCDEId)
        {
            var centreDeveloppementEntrepreneuriatlist = new SelectList(_centreDeveloppementEntrepreneuriatBusinessService.GetAll().Where(x => x.WilayaId == WilayaId && x.TypeCentreDeveloppementEntrepreneuriatId == TypeCDEId), "Id", "DesignationAr");
            return Json(centreDeveloppementEntrepreneuriatlist);
        }

        #endregion

    }
}
