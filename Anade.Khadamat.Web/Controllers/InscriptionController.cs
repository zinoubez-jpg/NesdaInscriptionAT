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
using Rework;

using Anade.Khadamat.Web.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Localization;


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

        private readonly Inscription_CommuneBusinesService _inscription_CommuneBusinesService;
        private readonly ILogger<InscriptionController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly PromoteurInscriptionBusinesService _promoteurInscriptionBusinessService;
        private readonly Detail_DossierInscriptionBusinesService _detail_DossierInscriptionBusinessService;
        private readonly AssociesInscriptionBusinesService _associesInscriptionBusinessService;
        private readonly Inscription_CategorieBusinesService _inscription_CategorieBusinesService;

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
                                      ReferentielBusinessService<CentreDeveloppementEntrepreneuriat, int> centreDeveloppementEntrepreneuriatBusinessService,

                                      Inscription_CommuneBusinesService inscription_CommuneBusinesService,
                                      ILogger<InscriptionController> logger,
                                      IWebHostEnvironment environment,
                                      PromoteurInscriptionBusinesService promoteurInscriptionBusinessService,
                                      Detail_DossierInscriptionBusinesService detail_DossierInscriptionBusinessService,
                                      AssociesInscriptionBusinesService associesInscriptionBusinessService,
                                      Inscription_CategorieBusinesService inscription_CategorieBusinesService
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

            _inscription_CommuneBusinesService = inscription_CommuneBusinesService;
            _logger = logger;
            _environment = environment;
            _promoteurInscriptionBusinessService = promoteurInscriptionBusinessService;
            _detail_DossierInscriptionBusinessService = detail_DossierInscriptionBusinessService;
            _associesInscriptionBusinessService = associesInscriptionBusinessService;
            _inscription_CategorieBusinesService = inscription_CategorieBusinesService;
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
            ViewBag.TypeInsId = typeInscriptionId;

            // Le processus diffère selon le type d'inscription :
            //    1- Création : contrôle de non-existence du promoteur.
            //    2- Extension (NESDA) : contrôle d'existence du promoteur.
            //    3- Extension (Hors NESDA) : contrôle de non-existence du promoteur.

            // Phase Extension
            if (typeInscriptionId == 2)
            {
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
                        if (CheckPromoteurExistenceByInfos(model))
                        {
                            var bResult = new BusinessResult();
                            bResult.Messages.Add(new MessageResult
                            {
                                Message = "تفيد نتائج التدقيق في نظامنا بوجود ملف مسجل مسبقًا باسمكم. لذا نرجو منكم الاطلاع على المعلومات التالية وإبلاغنا في حال وجود أي اختلاف: تاريخ الميلاد، بلدية الميلاد، ورقم شهادة الميلاد",
                                MessageType = MessageType.Error
                            });
                            TempData["Message"] = bResult.ToBootstrapAlerts();
                            InitializeListViewBags();
                            return View(model);
                        }
                    }

                    // Promoteur nesda (category == 2)
                    else
                    {
                        //apiResult = await CheckExtensionEligibilityAsync(model);
                        apiResult = CheckExtensionEligibilityByInfos(model);
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
            }

            // Phase Création
            else
            {
                // Dans l'inscription en ligne : [DBNesdaInscription6] --> CategoriesId = 1 --> Sans emploi
                model.CategorieId = 1;

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
                    InitializeListViewBags();
                    return View(model);
                }

                if (ModelState.IsValid)
                {

                    Inscription_Promoteur entity = new();

                    // Initialisations de l'entité Inscription_Promoteur

                    entity.NIN = model.NIN;
                    entity.NOM_P = model.NomFr;
                    entity.PRENOM_P = model.PrenomFr;

                    if (model.GenreId == 1)
                    {
                        entity.Inscription_SexeId = 2;
                    }
                    else
                    {
                        entity.Inscription_SexeId = 1;
                    }

                    entity.NOMJF_P = "";
                    entity.DDN_P = model.DateNaissance;

                    var commune = _communeBusinessService.GetById(model.CommuneNaissanceId);

                    entity.CommuneId = _inscription_CommuneBusinesService.GetByCode(commune.Code).Id;

                    entity.NUM_ACT_P = HttpUtility.UrlEncode(model.NIN.Substring(9, 5));

                    entity.ADR_P = "";
                    entity.TEL_P = model.NumTelephone;

                    var commune2 = _communeBusinessService.GetById(model.CommuneResidenceId);

                    entity.CommuneHabId = _inscription_CommuneBusinesService.GetByCode(commune2.Code).Id;

                    // entity.Inscription_CategorieId

                    var commune3 = _communeBusinessService.GetById(model.CommuneSeigeSocialId);

                    entity.Id_Commune_Siege_Social = _inscription_CommuneBusinesService.GetByCode(commune3.Code).Id;

                    entity.ADR_E = "";

                    // Dans l'inscription en ligne : [DBNesdaInscription6] --> IntituleDiplome c'est un champs Arabe,
                    // mais dans la base de données [INSCRIPTION] le champs Francais.

                    entity.SPECIALITE = model.IntituleDiplome;
                    entity.SPECIALITE = "...";

                    // Dans l'inscription en ligne : [INSCRIPTION] --> Inscription_PaysId = 4 --> Algérie.
                    entity.Inscription_PaysId = 4;

                    if (model.NiveauInstructionId == 1)
                    {
                        entity.Inscription_NiveauId = 5;
                    }
                    else
                    {
                        if (model.NiveauInstructionId == 2)
                        {
                            entity.Inscription_NiveauId = 6;
                        }
                        else
                        {
                            entity.Inscription_NiveauId = 1;
                        }
                    }

                    entity.EMAIL_P = model.Email;
                    entity.Mot_Passe = model.Password;

                    entity.CODE_P = "";
                    entity.Prenom_Pere = "";
                    entity.Nom_Prenom_Mere = "";
                    entity.Nom_P_Arab = model.NomAr;
                    entity.Prenom_P_Arab = model.PrenomAr;
                    entity.NomJF_P_Arab = "";
                    entity.Prenom_Pere_Arab = "";
                    entity.Nom_Prenom_Mer_Arab = "";
                    entity.Adr_P_Arab = model.AdressAr;

                    entity.Mot_Passe_ToSHA = entity.Mot_Passe.ToSHA(Crypto.SHA_Type.SHA512);
                    entity.Mot_Passe_RandomPassword_9_ToSHA = entity.Mot_Passe.ToSHA(Crypto.SHA_Type.SHA512);
                    entity.Ex_Detenu = false;

                    if (string.IsNullOrEmpty(entity.NIN) || entity.NIN.Length != 18 || !entity.NIN.All(char.IsDigit))
                    {
                        var bR = new BusinessResult();
                        var msgR = new MessageResult()
                        {
                            Message = "تأكد من أن الرقم التعريفي الوطني الوحيد الخاص بك, يتكون من 18 رقمًا بالضبط",
                            MessageType = MessageType.Error,
                        };

                        bR.Messages.Add(msgR);
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                    // Dans CreatePromoteur, ajoutez cette validation avant le ModelState.IsValid
                    if (_promoteurInscriptionBusinessService.GetByNIN(entity.NIN) != null)
                    {
                        var bR = new BusinessResult();
                        var msgR = new MessageResult()
                        {
                            Message = "الرقم التعريفي الوطني الوحيد الذي قمت بإدخاله مسجل مسبقًا في قاعدة البيانات",
                            MessageType = MessageType.Error,
                        };

                        bR.Messages.Add(msgR);
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                    // Validation de l'âge
                    if (entity.DDN_P != default)
                    {
                        if (!AgeHelper.IsAgeValid(entity.DDN_P))
                        {
                            var bR = new BusinessResult();
                            var msgR = new MessageResult()
                            {
                                Message = "يجب أن يتراوح العمر بين 18 و 55 سنة",
                                MessageType = MessageType.Error,
                            };

                            bR.Messages.Add(msgR);
                            TempData["Message"] = bR.ToBootstrapAlerts();
                            InitializeListViewBags();
                            return View(model);
                        }
                    }
                    else
                    {
                        var bR = new BusinessResult();
                        var msgR = new MessageResult()
                        {
                            Message = "يجب إدخال تاريخ الميلاد",
                            MessageType = MessageType.Error,
                        };

                        bR.Messages.Add(msgR);
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                    var user = await _userManager.FindByEmailAsync(entity.EMAIL_P);

                    if (user != null)
                    {
                        var bR = new BusinessResult();
                        var msgR = new MessageResult()
                        {
                            Message = "عنوان البريد الإلكتروني الذي أدخلته مستخدم في نظامنا بحساب آخر",
                            MessageType = MessageType.Error,
                        };

                        bR.Messages.Add(msgR);
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                    var apiBusinessResult = CreateInscriptionPromoteurCreation(entity, model);

                    if (!apiBusinessResult.Succeeded)
                    {
                        // Gérer l'erreur ici (afficher les messages dans TempData, etc.)
                        var bR = new BusinessResult();
                        foreach (var message in apiBusinessResult.Messages)
                        {
                            bR.Messages.Add(message);
                        }
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
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

                    if (model.CopieDiplomeCDEPdf.Length > 2048576)
                    {

                        var bResult = new BusinessResult();
                        bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        InitializeListViewBags();
                        return View(model);
                    }

                    if (!IsValidPdfFile(model.CopieDiplomeCDEPdf))
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

                    string copieDiplomeCDEPath = ProcessUploadedFile(model, model.CopieDiplomeCDEPdf, "_DpCDE", "DiplomeCDEFolder");

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
                        //antenne = _antenneBusinessService.GetSingle(x => x.Code == apiResult);
                        //agence = _agenceBusinessService.GetById(antenne.AgenceId);

                        var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                        antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
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
                        CopieDiplomeCDEPath = copieDiplomeCDEPath,

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
                        _ = _emailVerificationService.SendVerificationEmailAsync(NewInscription.Email, NewInscription.EmailVerificationToken);

                        return RedirectToAction(nameof(SuccesInscription));
                    }

                    TempData["Message"] = businessResult.ToBootstrapAlerts();
                    InitializeListViewBags();
                    return View(model);
                }

                else
                {
                    // Collecter toutes les erreurs de validation
                    var bR = new BusinessResult();
                    var msgR = new MessageResult()
                    {
                        Message = "واجهنا أخطاء في التحقق من البيانات",
                        MessageType = MessageType.Error,
                    };

                    bR.Messages.Add(msgR);
                    TempData["Message"] = bR.ToBootstrapAlerts();
                    InitializeListViewBags();
                    return View(model);
                }
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

        public ApiBusinessResult CreateInscriptionPromoteurCreation(Inscription_Promoteur entity, CreateInscriptionVM model)
        {
            ApiBusinessResult apiBusinessResult;

            if (entity == null)
                return new ApiBusinessResult(false) { Messages = { new MessageResult { Message = "الهوية غير موجودة", MessageType = MessageType.Error } } };

            var ninExistresult = _promoteurInscriptionBusinessService.Count(x => x.NIN == entity.NIN);

            if (ninExistresult == 0)
            {
                var emailExistresult = _promoteurInscriptionBusinessService.Count(x => x.EMAIL_P == entity.EMAIL_P);

                if (emailExistresult == 0)
                {
                    Inscription_Associes associer = new();
                    Inscription_Detail_Dossier detail_dossier = new();

                    detail_dossier.Date_Inscription = DateTime.Now;
                    detail_dossier.ADR_E = entity.ADR_E;
                    if (entity.Id_Commune_Siege_Social != null)
                        detail_dossier.Inscription_CommuneId = (int)entity.Id_Commune_Siege_Social;
                    else
                        detail_dossier.Inscription_CommuneId = 1;

                    detail_dossier.Adr_E_Arab = "";
                    detail_dossier.Inscription_ActiviteId = 1;
                    detail_dossier.Inscription_Type_FinancementId = 4;
                    detail_dossier.Inscription_PNR_SuppId = 1;

                    // TODO : Inscription_PhaseId pour la création extension;
                    detail_dossier.Inscription_PhaseId = 1;

                    // Ajouter la relation entre la table PROMOTEUR et Detail_dossier dans la table  associer
                    associer.Inscription_Promoteur = entity;
                    associer.Inscription_Detail_Dossier = detail_dossier;
                    associer.Inscription_QualiteId = 2;

                    // Pour test CHOMEUR valeur = 2
                    // associer.Inscription_CategorieId = entity.Inscription_CategorieId;
                    entity.Inscription_CategorieId = 2;
                    associer.Inscription_CategorieId = 2;


                    var inscription_CategoriePase = _inscription_CategorieBusinesService.GetById(entity.Inscription_CategorieId, _inscription_CategorieBusinesService.GetDefaultLoadProperties());

                    entity.Inscription_Categorie_PhaseId = inscription_CategoriePase.Inscription_PhaseId;

                    var businessResult = _promoteurInscriptionBusinessService.Add(entity);

                    if (businessResult.Succeeded)
                    {
                        apiBusinessResult = new ApiBusinessResult(businessResult.Succeeded) { };
                        apiBusinessResult.Result = entity;
                    }
                    else
                    {
                        apiBusinessResult = new ApiBusinessResult(false) { };
                        if (businessResult.Messages != null)
                        {
                            foreach (var message in businessResult.Messages)
                            {
                                apiBusinessResult.Messages.Add(new MessageResult
                                {
                                    Message = message.Message,
                                    MessageType = message.MessageType
                                });
                            }
                        }
                        else
                        {
                            apiBusinessResult.Messages.Add(new MessageResult
                            {
                                Message = "حدث خطأ غير معروف",
                                MessageType = MessageType.Error
                            });
                        }
                    }
                }
                else
                {
                    // CORRECTION: Retourner ApiBusinessResult au lieu de ViewResult
                    apiBusinessResult = new ApiBusinessResult(false);
                    apiBusinessResult.Messages.Add(new MessageResult
                    {
                        Message = "عنوان البريد الإلكتروني الذي أدخلته مستخدم في نظامنا بحساب آخر",
                        MessageType = MessageType.Error
                    });
                }
            }
            else
            {
                apiBusinessResult = new ApiBusinessResult(false);
                apiBusinessResult.Messages.Add(new MessageResult
                {
                    Message = "الرقم التعريفي الوطني مسجل مسبقاً",
                    MessageType = MessageType.Warning
                });
            }

            return apiBusinessResult;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmationCompte()
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
            var categories = _categorieBusinessService.GetAllFiltered(x => x.Id != 1);
            var typeFinancements = _typeFinnacementBusinessService.GetAll();
            var typesCentreDeveloppementEntrepreneuriat = _typeCentreDeveloppementEntrepreneuriat.GetAllFiltered(x => x.Id != 1);
            var centresDeveloppementEntrepreneuriat = _centreDeveloppementEntrepreneuriatBusinessService.GetAll();

            ViewBag.GenreId = new SelectList(genres, nameof(Genre.Id), nameof(Genre.DesignationAr));
            ViewBag.SituationFamilialeId = new SelectList(situationFamiliales, nameof(SituationFamiliale.Id), nameof(SituationFamiliale.DesignationAr));
            ViewBag.CommuneResidenceId = new SelectList(communes, nameof(Commune.Id), nameof(Commune.DesignationAr));
            ViewBag.WilayaResId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));
            ViewBag.NiveauInstructionId = new SelectList(niveauInstructions, nameof(NiveauInstruction.Id), nameof(NiveauInstruction.DesignationAr));
            ViewBag.TypeInscriptionId = new SelectList(typeInscription, nameof(TypeInscription.Id), nameof(TypeInscription.DesignationAr));
            ViewBag.CategorieId = new SelectList(categories, nameof(Categorie.Id), nameof(Categorie.DesignationAr));
            ViewBag.TypeFinancementId = new SelectList(typeFinancements, nameof(TypeFinancement.Id), nameof(TypeFinancement.DesignationAr));
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

        private bool CheckPromoteurExistenceByInfos(CreateInscriptionVM model)
        {
            Inscription_Promoteur promoteur = new();

            var encodedNumActP = HttpUtility.UrlEncode(model.NIN.Substring(9, 5)); ;
            var communeNaissanceNew = _communeBusinessService.GetById(model.CommuneNaissanceId);
            var communeNaissanceOld = _inscription_CommuneBusinesService.GetByCode(communeNaissanceNew.Code);

            promoteur.NIN = model.NIN;
            promoteur.DDN_P = model.DateNaissance;
            promoteur.NOM_P = model.NomFr;
            promoteur.PRENOM_P = model.PrenomFr;
            promoteur.NUM_ACT_P = "0"+encodedNumActP;
            promoteur.CommuneId = communeNaissanceOld.Id;
            promoteur.Inscription_Commune = communeNaissanceOld;

            return _promoteurInscriptionBusinessService.CheckPromoteurExistenceByInfos(promoteur);

        }

        private string CheckExtensionEligibilityByInfos(CreateInscriptionVM model)
        {
            Inscription_Promoteur promoteur = new();

            var encodedNumActP = HttpUtility.UrlEncode(model.NIN.Substring(9, 5)); ;
            var communeNaissanceNew = _communeBusinessService.GetById(model.CommuneNaissanceId);
            var communeNaissanceOld = _inscription_CommuneBusinesService.GetByCode(communeNaissanceNew.Code);

            promoteur.NIN = model.NIN;
            promoteur.DDN_P = model.DateNaissance;
            promoteur.NOM_P = model.NomFr;
            promoteur.PRENOM_P = model.PrenomFr;
            promoteur.NUM_ACT_P = "0"+encodedNumActP;
            promoteur.CommuneId = communeNaissanceOld.Id;
            promoteur.Inscription_Commune = communeNaissanceOld;

            return _promoteurInscriptionBusinessService.CheckExtensionEligibilityByInfos(promoteur);

        }

        [HttpGet]
        public JsonResult GetCentreDeveloppementEntrepreneuriatList(int WilayaId, int TypeCDEId)
        {
            var centreDeveloppementEntrepreneuriatlist = new SelectList(_centreDeveloppementEntrepreneuriatBusinessService.GetAll().Where(x => x.WilayaId == WilayaId && x.TypeCentreDeveloppementEntrepreneuriatId == TypeCDEId), "Id", "DesignationAr");
            return Json(centreDeveloppementEntrepreneuriatlist);
        }

        #endregion

        #region Méthodes à supprimer

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

        #endregion
    }
}
