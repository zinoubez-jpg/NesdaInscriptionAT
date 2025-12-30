using Anade.Business.Core;
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
using Microsoft.Extensions.Logging;
using Anade.Khadamat.Web.Services;

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
            //    A - Création : contrôle de non-existence du promoteur.
            //    B - Extension (NESDA) : contrôle d'existence du promoteur.
            //    C - Extension (Hors NESDA) : contrôle de non-existence du promoteur.

            // Phase Extension
            if (typeInscriptionId == TypesInscriptionId.Extension)
            {
                if (model.CategorieId == CategorieId.SansEmploi)
                {
                    var bResult = new BusinessResult();
                    bResult.Messages.Add(new MessageResult { Message = "يرجى إختيار نوع الفئة بشكل صحيح", MessageType = MessageType.Error });
                    TempData["Message"] = bResult.ToBootstrapAlerts();
                    InitializeListViewBags(model);
                    return View(model);
                }

                var isRecaptchaValid = _recaptchaService.ValidateRecaptchaAsync(model.RecaptchaResponse).Result;

                if (!isRecaptchaValid)
                {
                    ModelState.AddModelError("Recaptcha", "لم تتمكن من اجتياز اختبار الـ CAPTCHA. يرجى المحاولة مرة أخرى.");
                    InitializeListViewBags(model);

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
                    if (model.CategorieId != CategorieId.NESDA)
                    {
                        if (CheckPromoteurExistenceByInfos(model))
                        {
                            var bResult = new BusinessResult();
                            bResult.Messages.Add(new MessageResult
                            {
                                Message = "تفيد نتائج التدقيق في نظامنا بوجود ملف مسجل مسبقًا باسمكم. لذا نرجو منكم التحقق من جميع المعلومات, وإبلاغنا في حال وجود أي مشكل",
                                MessageType = MessageType.Error
                            });
                            TempData["Message"] = bResult.ToBootstrapAlerts();
                            InitializeListViewBags(model);
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
                            InitializeListViewBags(model);
                            return View(model);
                        }

                    }

                    // hckeing email
                    if (_context.Inscriptions.Any(u => u.Email == model.Email && u.IsEmailVerified))
                    {
                        ModelState.AddModelError("Email", "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر");
                        InitializeListViewBags(model);
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

                        InitializeListViewBags(model);

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
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    //string NamesOfCNIPdfPath = ProcessUploadedFileGuidName(model, model.CopieCNIPdf, "_CNI", "CNIFolder");

                    //string NamesOfCNIPdfPath = ProcessUploadedFileGuidName(model, model.CopieCNIPdf, "_CNI");

                    //string copieCNIPdfPath = ProcessUploadedFile(model, model.CopieCNIPdf, "_CNI", "CNIFolder");

                    string NamesOfCNIPdfPath = ProcessUploadedFile(model, model.CopieCNIPdf, "_CNI", "CNIFolder");

                    if (model.CopieDiplomeRCCFCAPdf.Length > 2048576)
                    {

                        var bResult = new BusinessResult();
                        bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        InitializeListViewBags(model);
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
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    //string NamesOfRCCFCAPdfPath = ProcessUploadedFileGuidName(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");

                    //string NamesOfRCCFCAPdfPath = ProcessUploadedFileGuidName(model, model.CopieDiplomeRCCFCAPdf, "_Dp");

                    //string copieDiplomeRCCFCAPdfPath = ProcessUploadedFile(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");

                    string NamesOfRCCFCAPdfPath = ProcessUploadedFile(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");

                    var communeSS = _communeBusinessService.GetById(model.CommuneSeigeSocialId, _communeBusinessService.GetDefaultLoadProperties());

                    Antenne antenne;
                    Agence agence;

                    if (model.CategorieId != CategorieId.NESDA)
                    {
                        var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                        antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
                        agence = _agenceBusinessService.GetById(antenne.AgenceId);

                    }
                    else
                    {
                        //var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == apiResult);
                        //antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
                        //agence = _agenceBusinessService.GetById(antenne.AgenceId);

                        antenne = _antenneBusinessService.GetSingle(x => x.Code == apiResult);
                        agence = _agenceBusinessService.GetById(antenne.AgenceId);
                    }

                    List<Document> documents = new();

                    Document documentCNI = new();

                    documentCNI.TypeDocumentId = TypeDocumentId.CNI;
                    documentCNI.FilePath = NamesOfCNIPdfPath;

                    Document documentDiplome = new();

                    documentDiplome.TypeDocumentId = TypeDocumentId.Diplome;
                    documentDiplome.FilePath = NamesOfRCCFCAPdfPath;

                    documents.Add(documentCNI);
                    documents.Add(documentDiplome);

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
                        //CopieCNIPath = NamesOfCNIPdfPath,
                        //CopieDiplomeRCCFCAPath = NamesOfRCCFCAPdfPath,
                        AntenneId = antenne.Id,
                        Anntenne = antenne.Code,
                        Agence = agence.Code,

                        ActiviteId = 1,
                        PasswordHashInit = model.Password,

                        CentreDeveloppementEntrepreneuriatId = model.CentreDeveloppementEntrepreneuriatId,
                        Documents = documents
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
                    InitializeListViewBags(model);
                    return View(model);

                }
            }

            // Phase Création
            else
            {
                // Dans l'inscription en ligne : [DBNesdaInscription6] --> CategoriesId = 1 --> Sans emploi
                model.CategorieId = CategorieId.SansEmploi;

                var isRecaptchaValid = _recaptchaService.ValidateRecaptchaAsync(model.RecaptchaResponse).Result;

                if (!isRecaptchaValid)
                {
                    ModelState.AddModelError("Recaptcha", "لم تتمكن من اجتياز اختبار الـ CAPTCHA. يرجى المحاولة مرة أخرى.");
                    InitializeListViewBags(model);

                    var bR = new BusinessResult();
                    var msgR = new MessageResult()
                    {
                        Message = "لم تتمكن من اجتياز اختبار الـ CAPTCHA. يرجى المحاولة مرة أخرى.",
                        MessageType = MessageType.Error,
                    };

                    bR.Messages.Add(msgR);
                    TempData["Message"] = bR.ToBootstrapAlerts();
                    InitializeListViewBags(model);
                    return View(model);
                }

                if (model.CentreDeveloppementEntrepreneuriatId == 1)
                {
                    ModelState.AddModelError("CDE", "لم تقم باختياز مركز تطوير المقاولاتية. يرجى المحاولة مرة أخرى.");
                    InitializeListViewBags(model);

                    var bR = new BusinessResult();
                    var msgR = new MessageResult()
                    {
                        Message = "لم تقم باختياز مركز تطوير المقاولاتية. يرجى المحاولة مرة أخرى.",
                        MessageType = MessageType.Error,
                    };

                    bR.Messages.Add(msgR);
                    TempData["Message"] = bR.ToBootstrapAlerts();
                    InitializeListViewBags(model);
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

                    entity.CommuneId = _inscription_CommuneBusinesService.GetByCode(commune.NouveauCode).Id;

                    entity.NUM_ACT_P = HttpUtility.UrlEncode(model.NIN.Substring(9, 5));

                    entity.ADR_P = "";
                    entity.TEL_P = model.NumTelephone;

                    var commune2 = _communeBusinessService.GetById(model.CommuneResidenceId);

                    entity.CommuneHabId = _inscription_CommuneBusinesService.GetByCode(commune2.NouveauCode).Id;

                    // entity.Inscription_CategorieId

                    var commune3 = _communeBusinessService.GetById(model.CommuneSeigeSocialId);

                    entity.Id_Commune_Siege_Social = _inscription_CommuneBusinesService.GetByCode(commune3.NouveauCode).Id;

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
                        InitializeListViewBags(model);
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
                        InitializeListViewBags(model);
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
                            InitializeListViewBags(model);
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
                        InitializeListViewBags(model);
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
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    ApiBusinessResult apiBusinessResult = CreateInscriptionPromoteurCreation(entity, model);


                    if (!apiBusinessResult.Succeeded)
                    {
                        // Gérer l'erreur ici (afficher les messages dans TempData, etc.)
                        var bR = new BusinessResult();
                        foreach (var message in apiBusinessResult.Messages)
                        {
                            bR.Messages.Add(message);
                        }
                        TempData["Message"] = bR.ToBootstrapAlerts();
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    // hckeing email
                    if (_context.Inscriptions.Any(u => u.Email == model.Email && u.IsEmailVerified))
                    {
                        ModelState.AddModelError("Email", "البريد الإلكتروني الذي أدخلته مرتبط بحساب موجود. يرجى استخدام بريد إلكتروني آخر");
                        InitializeListViewBags(model);
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

                        InitializeListViewBags(model);

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
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    string NamesOfCNIPdfPath = ProcessUploadedFile(model, model.CopieCNIPdf, "_CNI", "CNIFolder");

                    if (model.CopieDiplomeRCCFCAPdf.Length > 2048576)
                    {

                        var bResult = new BusinessResult();
                        bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });
                        TempData["Message"] = bResult.ToBootstrapAlerts();
                        InitializeListViewBags(model);
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
                        InitializeListViewBags(model);
                        return View(model);
                    }

                    string NamesOfRCCFCAPdfPath = ProcessUploadedFile(model, model.CopieDiplomeRCCFCAPdf, "_Dp", "DiplomeFolder");

                    string NamesOfCDEPath = null;

                    // Vérifier si un fichier a été uploadé (champ optionnel)
                    if (model.CopieDiplomeCDEPdf != null && model.CopieDiplomeCDEPdf.Length > 0)
                    {
                        // Valider seulement si un fichier est présent
                        if (model.CopieDiplomeCDEPdf.Length > 2048576)
                        {
                            var bResult = new BusinessResult();
                            bResult.Messages.Add(new MessageResult { Message = "حجم الملف يتجاوز الحد المسموح به", MessageType = MessageType.Error });
                            TempData["Message"] = bResult.ToBootstrapAlerts();
                            InitializeListViewBags(model);
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
                            InitializeListViewBags(model);
                            return View(model);
                        }

                        NamesOfCDEPath = ProcessUploadedFile(model, model.CopieDiplomeCDEPdf, "_DpCDE", "DiplomeCDEFolder");
                    }

                    var communeSS = _communeBusinessService.GetById(model.CommuneSeigeSocialId, _communeBusinessService.GetDefaultLoadProperties());

                    Antenne antenne;
                    Agence agence;

                    if (model.CategorieId != CategorieId.NESDA)
                    {
                        var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                        antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
                        agence = _agenceBusinessService.GetById(antenne.AgenceId);
                    }

                    else
                    {
                        var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
                        antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
                        agence = _agenceBusinessService.GetById(antenne.AgenceId);
                    }

                    List<Document> documents = new();

                    Document documentCNI = new();

                    documentCNI.TypeDocumentId = TypeDocumentId.CNI;
                    documentCNI.FilePath = NamesOfCNIPdfPath;

                    Document documentDiplome = new();

                    documentDiplome.TypeDocumentId = TypeDocumentId.Diplome;
                    documentDiplome.FilePath = NamesOfRCCFCAPdfPath;

                    documents.Add(documentCNI);
                    documents.Add(documentDiplome);

                    // Ajouter le document CDE seulement si le fichier a été uploadé
                    if (NamesOfCDEPath != null)
                    {
                        Document documentDiplomeCDE = new();
                        documentDiplomeCDE.TypeDocumentId = TypeDocumentId.DiplomeCDE;
                        documentDiplomeCDE.FilePath = NamesOfCDEPath;

                        documents.Add(documentDiplomeCDE);
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

                        AntenneId = antenne.Id,
                        Anntenne = antenne.Code,
                        Agence = agence.Code,

                        ActiviteId = 1,
                        PasswordHashInit = model.Password,

                        CentreDeveloppementEntrepreneuriatId = model.CentreDeveloppementEntrepreneuriatId,
                        Documents = documents
                    };

                    var businessResult = _inscriptionBusinessService.Add(NewInscription);

                    if (businessResult.Succeeded)
                    {
                        _ = _emailVerificationService.SendVerificationEmailAsync(NewInscription.Email, NewInscription.EmailVerificationToken);

                        return RedirectToAction(nameof(SuccesInscription));
                    }

                    TempData["Message"] = businessResult.ToBootstrapAlerts();
                    InitializeListViewBags(model);
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
                    InitializeListViewBags(model);
                    return View(model);
                }
            }

            InitializeListViewBags(model);
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

        public ApiBusinessResult CreateInscriptionPromoteurCreationTest(Inscription_Promoteur entity, CreateInscriptionVM model)
        {
            ApiBusinessResult apiBusinessResult = new(false)
            {
                Messages =
                {
                    new MessageResult
                    {
                        Message = "حدث خطأ غير معروف",
                        MessageType = MessageType.Error
                    }
                }
            };

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
                    associer.Inscription_QualiteId = QualiteId.Gérant;

                    // Pour test CHOMEUR valeur = 2
                    // associer.Inscription_CategorieId = entity.Inscription_CategorieId;
                    entity.Inscription_CategorieId = CategorieId.NESDA;
                    associer.Inscription_CategorieId = CategorieId.NESDA;

                    var inscription_CategoriePase = _inscription_CategorieBusinesService.GetById(entity.Inscription_CategorieId, _inscription_CategorieBusinesService.GetDefaultLoadProperties());

                    entity.Inscription_Categorie_PhaseId = inscription_CategoriePase.Inscription_PhaseId;

                    CentreDeveloppementEntrepreneuriat cdeEle = _centreDeveloppementEntrepreneuriatBusinessService.GetById(model.CentreDeveloppementEntrepreneuriatId);

                    var businessResult = _associesInscriptionBusinessService.AddInscription(associer, cdeEle);

                    if (businessResult.Succeeded)
                    {
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

        // Dans CreateInscriptionPromoteurCreation méthode, ajoutez la journalisation :

        public ApiBusinessResult CreateInscriptionPromoteurCreation(Inscription_Promoteur entity, CreateInscriptionVM model)
        {
            ApiBusinessResult apiBusinessResult = new(false)
            {
                Messages =
                {
                    new MessageResult
                    {
                        Message = "حدث خطأ غير معروف",
                        MessageType = MessageType.Error
                    }
                }
            };

            // Log avant l'appel de la méthode
            _logger.LogInformation("Début de CreateInscriptionPromoteurCreation pour le promoteur avec NIN: {NIN} et Email: {Email}",
                entity?.NIN, entity?.EMAIL_P);

            try
            {
                if (entity == null)
                {
                    _logger.LogError("Entité Inscription_Promoteur est null dans CreateInscriptionPromoteurCreation");
                    return new ApiBusinessResult(false)
                    {
                        Messages = { new MessageResult { Message = "الهوية غير موجودة", MessageType = MessageType.Error } }
                    };
                }

                var ninExistresult = _promoteurInscriptionBusinessService.Count(x => x.NIN == entity.NIN);
                _logger.LogDebug("Vérification NIN: {NIN} - Résultat: {Count}", entity.NIN, ninExistresult);

                if (ninExistresult == 0)
                {
                    var emailExistresult = _promoteurInscriptionBusinessService.Count(x => x.EMAIL_P == entity.EMAIL_P);
                    _logger.LogDebug("Vérification Email: {Email} - Résultat: {Count}", entity.EMAIL_P, emailExistresult);

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
                        detail_dossier.Inscription_PhaseId = 1;

                        associer.Inscription_Promoteur = entity;
                        associer.Inscription_Detail_Dossier = detail_dossier;
                        associer.Inscription_QualiteId = QualiteId.Gérant;
                        entity.Inscription_CategorieId = CategorieId.NESDA;
                        associer.Inscription_CategorieId = CategorieId.NESDA;

                        var inscription_CategoriePase = _inscription_CategorieBusinesService.GetById(
                            entity.Inscription_CategorieId,
                            _inscription_CategorieBusinesService.GetDefaultLoadProperties());

                        entity.Inscription_Categorie_PhaseId = inscription_CategoriePase.Inscription_PhaseId;

                        CentreDeveloppementEntrepreneuriat cdeEle = _centreDeveloppementEntrepreneuriatBusinessService
                            .GetById(model.CentreDeveloppementEntrepreneuriatId);

                        // Log avant AddInscription
                        _logger.LogInformation("Appel de AddInscription pour l'associé - Promoteur: {CodePromoteur}, CDE: {CdeId}",
                            entity.CODE_P, model.CentreDeveloppementEntrepreneuriatId);

                        var businessResult = _associesInscriptionBusinessService.AddInscription(associer, cdeEle);

                        // Log après AddInscription
                        _logger.LogInformation("Résultat de AddInscription: {Succeeded} - Messages: {MessageCount}",
                            businessResult.Succeeded, businessResult.Messages?.Count ?? 0);

                        if (businessResult.Succeeded)
                        {
                            apiBusinessResult = new ApiBusinessResult(businessResult.Succeeded) { };
                            apiBusinessResult.Result = entity;
                            _logger.LogInformation("AddInscription réussi pour le promoteur: {NIN}", entity.NIN);
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
                                _logger.LogError("AddInscription échoué pour {NIN}: {ErrorMessages}",
                                    entity.NIN, string.Join("; ", businessResult.Messages.Select(m => m.Message)));
                            }
                            else
                            {
                                apiBusinessResult.Messages.Add(new MessageResult
                                {
                                    Message = "حدث خطأ غير معروف",
                                    MessageType = MessageType.Error
                                });
                                _logger.LogError("AddInscription échoué sans messages pour {NIN}", entity.NIN);
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Email déjà existant: {Email}", entity.EMAIL_P);
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
                    _logger.LogWarning("NIN déjà existant: {NIN}", entity.NIN);
                    apiBusinessResult = new ApiBusinessResult(false);
                    apiBusinessResult.Messages.Add(new MessageResult
                    {
                        Message = "الرقم التعريفي الوطني مسجل مسبقاً",
                        MessageType = MessageType.Warning
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur inattendue dans CreateInscriptionPromoteurCreation pour NIN: {NIN}", entity?.NIN);
                apiBusinessResult = new ApiBusinessResult(false);
                apiBusinessResult.Messages.Add(new MessageResult
                {
                    Message = "حدث خطأ غير متوقع في النظام",
                    MessageType = MessageType.Error
                });
            }

            _logger.LogInformation("Fin de CreateInscriptionPromoteurCreation - Succès: {Succeeded}", apiBusinessResult.Succeeded);
            return apiBusinessResult;
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmationCompte()
        {
            return View();
        }

        #region Helper Methods
        private void InitializeListViewBags(CreateInscriptionVM model = null)
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

            // Pour chaque dropdown de commune, créer la liste appropriée
            if (model != null)
            {
                // Communes pour la wilaya de naissance
                if (model.WilayaNaissanceId > 0)
                {
                    var communesNaissance = _communeBusinessService.GetAll()
                        .Where(x => x.WilayaId == model.WilayaNaissanceId)
                        .ToList();
                    ViewBag.CommunesNaissance = new SelectList(communesNaissance,
                        nameof(Commune.Id), nameof(Commune.DesignationAr),
                        model.CommuneNaissanceId);
                }

                // Communes pour la wilaya de résidence
                if (model.WilayaResidenceId > 0)
                {
                    var communesResidence = _communeBusinessService.GetAll()
                        .Where(x => x.WilayaId == model.WilayaResidenceId)
                        .ToList();
                    ViewBag.CommunesResidence = new SelectList(communesResidence,
                        nameof(Commune.Id), nameof(Commune.DesignationAr),
                        model.CommuneResidenceId);
                }

                // Communes pour la wilaya du siège social
                if (model.WilayaSeigeSocialId > 0)
                {
                    var communesSiege = _communeBusinessService.GetAll()
                        .Where(x => x.WilayaId == model.WilayaSeigeSocialId)
                        .ToList();
                    ViewBag.CommunesSiege = new SelectList(communesSiege,
                        nameof(Commune.Id), nameof(Commune.DesignationAr),
                        model.CommuneSeigeSocialId);
                }
            }

            ViewBag.TypeCentreDeveloppementEntrepreneuriatId = new SelectList(
                typesCentreDeveloppementEntrepreneuriat,
                nameof(TypeCentreDeveloppementEntrepreneuriat.Id),
                nameof(TypeCentreDeveloppementEntrepreneuriat.DesignationAr));

            ViewBag.WilayaCentreDeveloppementEntrepreneuriatId = new SelectList(
                wilayas,
                nameof(Wilaya.Id),
                nameof(Wilaya.DisplayNameAr));

            // Initialiser avec liste vide ou complète selon le modèle
            if (model != null && model.WilayaCentreDeveloppementEntrepreneuriatId > 0 &&
                model.TypeCentreDeveloppementEntrepreneuriatId > 0)
            {
                // Filtrer les centres par wilaya ET type
                var centresFiltres = centresDeveloppementEntrepreneuriat
                    .Where(x => x.WilayaId == model.WilayaCentreDeveloppementEntrepreneuriatId &&
                               x.TypeCentreDeveloppementEntrepreneuriatId == model.TypeCentreDeveloppementEntrepreneuriatId)
                    .ToList();

                ViewBag.CentreDeveloppementEntrepreneuriatId = new SelectList(
                    centresFiltres,
                    nameof(CentreDeveloppementEntrepreneuriat.Id),
                    nameof(CentreDeveloppementEntrepreneuriat.DesignationAr),
                    model.CentreDeveloppementEntrepreneuriatId);
            }
            else
            {
                // Liste vide ou tous les centres
                ViewBag.CentreDeveloppementEntrepreneuriatId = new SelectList(
                    new List<CentreDeveloppementEntrepreneuriat>(),
                    nameof(CentreDeveloppementEntrepreneuriat.Id),
                    nameof(CentreDeveloppementEntrepreneuriat.DesignationAr));
            }
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
                // 📁 Exemple : créer le dossier dans le répertoire racine de l’application (pas dans wwwroot)
                string appRootPath = _hostingEnvironment.ContentRootPath;

                // Crée le dossier de destination (en dehors de wwwroot)
                string uploadsFolder = Path.Combine(appRootPath, "ProtectedFiles", folderName);

                // Crée le dossier s’il n’existe pas
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // 🧾 Nom du fichier
                uniqueFileName = $"{model.NIN}{fileNameSuffix}{model.Id}.pdf";

                // 🔗 Chemin complet du fichier
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 💾 Sauvegarde du fichier
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
            var communeNaissanceOld = _inscription_CommuneBusinesService.GetByCode(communeNaissanceNew.NouveauCode);

            promoteur.NIN = model.NIN;
            promoteur.DDN_P = model.DateNaissance;
            promoteur.NOM_P = model.NomFr;
            promoteur.PRENOM_P = model.PrenomFr;
            promoteur.NUM_ACT_P = "0" + encodedNumActP;
            promoteur.CommuneId = communeNaissanceOld.Id;
            promoteur.Inscription_Commune = communeNaissanceOld;

            return _promoteurInscriptionBusinessService.CheckPromoteurExistenceByInfos(promoteur);
        }

        private string CheckExtensionEligibilityByInfos(CreateInscriptionVM model)
        {
            Inscription_Promoteur promoteur = new();

            var encodedNumActP = HttpUtility.UrlEncode(model.NIN.Substring(9, 5)); ;
            var communeNaissanceNew = _communeBusinessService.GetById(model.CommuneNaissanceId);
            var communeNaissanceOld = _inscription_CommuneBusinesService.GetByCode(communeNaissanceNew.NouveauCode);

            promoteur.NIN = model.NIN;
            promoteur.DDN_P = model.DateNaissance;
            promoteur.NOM_P = model.NomFr;
            promoteur.PRENOM_P = model.PrenomFr;
            promoteur.NUM_ACT_P = "0" + encodedNumActP;
            promoteur.CommuneId = communeNaissanceOld.Id;
            promoteur.Inscription_Commune = communeNaissanceOld;

            return _promoteurInscriptionBusinessService.CheckExtensionEligibilityByInfos(promoteur);
        }

        [HttpGet]
        public JsonResult GetCentreDeveloppementEntrepreneuriatList2(int WilayaId, int TypeCDEId)
        {
            var centreDeveloppementEntrepreneuriatlist = new SelectList(_centreDeveloppementEntrepreneuriatBusinessService.GetAll().Where(x => x.WilayaId == WilayaId && x.TypeCentreDeveloppementEntrepreneuriatId == TypeCDEId), "Id", "DesignationAr");
            return Json(centreDeveloppementEntrepreneuriatlist);
        }

        [HttpGet]
        public JsonResult GetCentreDeveloppementEntrepreneuriatList(int WilayaId, int TypeCDEId, int? selectedId = null)
        {
            var centres = _centreDeveloppementEntrepreneuriatBusinessService.GetAll()
                .Where(x => x.WilayaId == WilayaId && x.TypeCentreDeveloppementEntrepreneuriatId == TypeCDEId)
                .Select(c => new
                {
                    value = c.Id,
                    text = c.DesignationAr,
                    selected = (selectedId.HasValue && c.Id == selectedId.Value)
                })
                .ToList();

            return Json(centres);
        }

        #endregion

    }
}
