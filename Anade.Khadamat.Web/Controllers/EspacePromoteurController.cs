using Anade.Business.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Authorize]
    public class EspacePromoteurController : Controller
    {
        public readonly InscriptionBusinessService _inscriptionBusinessService;

        public readonly ReferentielBusinessService<Commune, int> _communeBusinessService;
        public readonly ReferentielBusinessService<Wilaya, int> _wilayaBusinessService;
        public readonly ReferentielBusinessService<Genre, int> _genreBusinessService;
        public readonly ReferentielBusinessService<SituationFamiliale, int> _situationFamilialeBusinessService;
        public readonly ReferentielBusinessService<NiveauInstruction, int> _niveauInstructionBusinessService;
        public readonly ReferentielBusinessService<Formation, int> _formationBusinessService;
        private readonly ReferentielBusinessService<Antenne, int> _antenneBusinessService;
        private readonly ReferentielBusinessService<Agence, int> _agenceBusinessService;
        private readonly CirconscriptionBusinessService _circonscriptionBusinessService;

        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly UserStructureService _userStructureService;
        private readonly TypeUserServices _typeUserServices;
        private readonly StructureService _structureService;

        private readonly PromoteurInscriptionBusinesService _promoteurInscriptionBusinessService;
        private readonly Detail_DossierInscriptionBusinesService _detail_DossierInscriptionBusinessService;
        private readonly AssociesInscriptionBusinesService _associesInscriptionBusinessService;

        public EspacePromoteurController(InscriptionBusinessService inscriptionBusinessService,
                                          UserManager<User> userManager,
                                          RoleManager<Role> roleManager,
                                          UserService userService,
                                          UserStructureService userStructureService,
                                          StructureService structureService,
                                          TypeUserServices typeUserServices,
                                          ReferentielBusinessService<Commune, int> communeBusinessService,
                                          ReferentielBusinessService<Wilaya, int> wilayaBusinessService,
                                          ReferentielBusinessService<Genre, int> genreBusinessService,
                                          ReferentielBusinessService<SituationFamiliale, int> situationFamilialeBusinessService,
                                          ReferentielBusinessService<NiveauInstruction, int> niveauInstructionBusinessService,
                                          ReferentielBusinessService<Formation, int> formationBusinessService,
                                          ReferentielBusinessService<Antenne, int> antenneBusinessService,
                                          ReferentielBusinessService<Agence, int> agenceBusinessService,
                                          CirconscriptionBusinessService circonscriptionBusinessService,
                                          PromoteurInscriptionBusinesService promoteurInscriptionBusinessService,
                                          Detail_DossierInscriptionBusinesService detail_DossierInscriptionBusinessService,
                                          AssociesInscriptionBusinesService associesInscriptionBusinessService
                                         )
        {
            _inscriptionBusinessService = inscriptionBusinessService;
            _communeBusinessService = communeBusinessService;
            _wilayaBusinessService = wilayaBusinessService;
            _genreBusinessService = genreBusinessService;
            _situationFamilialeBusinessService = situationFamilialeBusinessService;
            _niveauInstructionBusinessService = niveauInstructionBusinessService;
            _typeUserServices = typeUserServices;
            _formationBusinessService = formationBusinessService;
            _antenneBusinessService = antenneBusinessService;
            _agenceBusinessService = agenceBusinessService;
            _circonscriptionBusinessService = circonscriptionBusinessService;
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
            _userStructureService = userStructureService;
            _structureService = structureService;
            _promoteurInscriptionBusinessService = promoteurInscriptionBusinessService;
            _detail_DossierInscriptionBusinessService = detail_DossierInscriptionBusinessService;
            _associesInscriptionBusinessService = associesInscriptionBusinessService;
        }
        public IActionResult DetailsPromoteur1()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            // Remplacer GetSingle par GetMany + FirstOrDefault
            var inscriptions = _inscriptionBusinessService.GetAllFiltered(x => x.Email == user.Email, _inscriptionBusinessService.GetDefaultLoadProperties());
            var inscrit = inscriptions.FirstOrDefault();

            // Loguer les doublons
            if (inscriptions.Count() > 1)
            {
                // Utilisez votre logger ici
                Console.WriteLine($"ATTENTION: Email {user.Email} a {inscriptions.Count} inscriptions");
            }

            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }

            InitializeListViewBags(inscrit);

            ViewBag.Wilayas = _wilayaBusinessService.GetAll();
            ViewBag.Communes = _communeBusinessService.GetAll();

            foreach (var item in inscrit.SessionFormations)
            {
                item.Formation = _formationBusinessService.GetById(item.FormationId);
            }

            return View(inscrit);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSiegeSocial1(int Id, string AddresseArSeigeSocial, int CommuneSiegeSocialId)
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            var inscriptions = _inscriptionBusinessService
                .GetAllFiltered(x => x.Email == user.Email && x.Id == Id,
                                _inscriptionBusinessService.GetDefaultLoadProperties());

            var inscrit = inscriptions.FirstOrDefault();

            if (inscrit == null) return NotFound();
            if (inscrit.Annulee) return Content("Votre inscription est annulée.");
            if (inscrit.Validee) return Content("Inscription déjà validée, modification impossible.");

            inscrit.AddresseArSeigeSocial = AddresseArSeigeSocial;
            inscrit.CommuneSeigeSocialId = CommuneSiegeSocialId;

            _inscriptionBusinessService.Update(inscrit);

            TempData["Success"] = "تم تحديث بيانات المقر الإجتماعي بنجاح.";
            return RedirectToAction(nameof(DetailsPromoteur));
        }
        
        public IActionResult DetailsPromoteur()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            var inscriptions = _inscriptionBusinessService
                .GetAllFiltered(x => x.Email == user.Email,
                                _inscriptionBusinessService.GetDefaultLoadProperties());
            var inscrit = inscriptions.FirstOrDefault();

            if (inscrit == null) return NotFound();
            if (inscrit.Annulee) return Content("Votre inscription est annulée.");
            if (inscrit.Validee) return Content("Inscription déjà validée, modification impossible.");

            // Charger UNE SEULE fois les référentiels
            var wilayas = _wilayaBusinessService.GetAll();
            var communes = _communeBusinessService.GetAll();

            // Construire les SelectList à partir de ces instances
            ViewBag.WilayaSeigeSocialId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));
            ViewBag.CommuneSeigeSocialId = new SelectList(
                inscrit.WilayaSeigeSocialId > 0
                    ? communes.Where(c => c.WilayaId == inscrit.WilayaSeigeSocialId).ToList()
                    : communes,
                nameof(Commune.Id), nameof(Commune.DesignationAr),
                inscrit.CommuneSeigeSocialId);

            // NE PAS recharger les Formation via GetById (elles sont dans le graphe déjà)
            // ... ou charger en une seule fois via GetAll()
            var formations = _formationBusinessService.GetAll().ToDictionary(f => f.Id);

            foreach (var item in inscrit.SessionFormations)
            {
                if (formations.TryGetValue(item.FormationId, out var f))
                    item.Formation = f;
            }

            return View(inscrit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSiegeSocial2(int Id, string AddresseArSeigeSocial, int CommuneSeigeSocialId)
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            // Charger SANS tracking et SANS navigation (juste l'entité)
            var inscrit = _inscriptionBusinessService
                .GetSingle(x => x.Email == user.Email && x.Id == Id); // SANS navigationProperties

            if (inscrit == null) return NotFound();
            if (inscrit.Annulee) return Content("Votre inscription est annulée.");
            if (inscrit.Validee) return Content("Inscription déjà validée, modification impossible.");

            inscrit.AddresseArSeigeSocial = AddresseArSeigeSocial;
            inscrit.CommuneSeigeSocialId = CommuneSeigeSocialId;

            Antenne antenne;
            Agence agence;

            var communeSS = _communeBusinessService.GetById(CommuneSeigeSocialId, _communeBusinessService.GetDefaultLoadProperties());
            var circonscription = _circonscriptionBusinessService.GetSingle(x => x.CodeCommune == communeSS.Code);
            antenne = _antenneBusinessService.GetSingle(x => x.Code == circonscription.CodeAntenne);
            agence = _agenceBusinessService.GetById(antenne.AgenceId);


            inscrit.AntenneId = antenne.Id;
            inscrit.Anntenne = antenne.Code;
            inscrit.Agence = agence.Code;
            inscrit.WilayaSeigeSocialId = communeSS.WilayaId;

            // Mise à jour ciblée sans graphe
            _inscriptionBusinessService.Update(inscrit);
            // => Update() doit attacher une entité sans navigation
            TempData["Success"] = "تم تحديث بيانات المقر الإجتماعي بنجاح.";
            return RedirectToAction(nameof(DetailsPromoteur));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSiegeSocial(int Id, string AddresseArSeigeSocial, int CommuneSeigeSocialId)
        {
            try
            {
                var user = _userService.GetUserEagerLoadedAsync(User).Result;

                var inscrit = _inscriptionBusinessService
                    .GetSingle(x => x.Email == user.Email && x.Id == Id);

                if (inscrit == null)
                {
                    TempData["Error"] = "لم يتم العثور على التسجيل.";
                    return RedirectToAction(nameof(DetailsPromoteur));
                }

                if (inscrit.Annulee)
                {
                    TempData["Error"] = "تسجيلك ملغى، لا يمكن التعديل.";
                    return RedirectToAction(nameof(DetailsPromoteur));
                }

                if (inscrit.Validee)
                {
                    TempData["Error"] = "التسجيل تم التحقق منه، لا يمكن التعديل.";
                    return RedirectToAction(nameof(DetailsPromoteur));
                }

                // Mise à jour des champs
                inscrit.AddresseArSeigeSocial = AddresseArSeigeSocial;
                inscrit.CommuneSeigeSocialId = CommuneSeigeSocialId;

                var communeSS = _communeBusinessService.GetById(
                    CommuneSeigeSocialId,
                    _communeBusinessService.GetDefaultLoadProperties());

                var circonscription = _circonscriptionBusinessService.GetSingle(
                    x => x.CodeCommune == communeSS.Code);

                var antenne = _antenneBusinessService.GetSingle(
                    x => x.Code == circonscription.CodeAntenne);

                var agence = _agenceBusinessService.GetById(antenne.AgenceId);

                inscrit.AntenneId = antenne.Id;
                inscrit.Anntenne = antenne.Code;
                inscrit.Agence = agence.Code;
                inscrit.WilayaSeigeSocialId = communeSS.WilayaId;

                var result = _inscriptionBusinessService.Update(inscrit);

                if (result.Succeeded)
                {
                    TempData["Success"] = "تم تحديث بيانات المقر الإجتماعي بنجاح.";
                }
                else
                {
                    TempData["Error"] = "فشل تحديث بيانات المقر الإجتماعي. يرجى المحاولة مرة أخرى.";
                }

                return RedirectToAction(nameof(DetailsPromoteur));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ غير متوقع: " + ex.Message;
                return RedirectToAction(nameof(DetailsPromoteur));
            }
        }

        public IActionResult SuiviDossier()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            // Version avec email vérifié en priorité
            var inscriptions = _inscriptionBusinessService.GetAllFiltered(x => x.Email == user.Email, _inscriptionBusinessService.GetDefaultLoadProperties());

            // Priorité à celle avec email vérifié
            var inscrit = inscriptions.FirstOrDefault(x => x.IsEmailVerified) ?? inscriptions.FirstOrDefault();

            if (inscriptions.Count() > 1)
            {
                Console.WriteLine($"ATTENTION: Email {user.Email} a {inscriptions.Count} inscriptions");
            }

            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }

            var WilayaNais = _wilayaBusinessService.GetById(inscrit.CommuneNaissance.WilayaId);
            var wilayaRes = _wilayaBusinessService.GetById(inscrit.CommuneResidence.WilayaId);
            var wilayaSS = _wilayaBusinessService.GetById(inscrit.CommuneSiegeSocial.WilayaId);

            inscrit.CommuneNaissance.Wilaya = WilayaNais;
            inscrit.CommuneResidence.Wilaya = wilayaRes;
            inscrit.CommuneSiegeSocial.Wilaya = wilayaSS;

            return View("SuiviDossier", inscrit);
        }

        public IActionResult ATFormation()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;

            var inscriptions = _inscriptionBusinessService.GetAllFiltered(x => x.Email == user.Email, _inscriptionBusinessService.GetDefaultLoadProperties());
            var inscrit = inscriptions.FirstOrDefault();

            if (inscriptions.Count() > 1)
            {
                Console.WriteLine($"ATTENTION: Email {user.Email} a {inscriptions.Count} inscriptions");
            }

            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }

            foreach (var item in inscrit.SessionFormations)
            {
                item.Formation = _formationBusinessService.GetById(item.FormationId);
            }

            return View(inscrit);
        }

        private void InitializeListViewBags(Inscription model = null)
        {
            var communes = _communeBusinessService.GetAll();
            var wilayas = _wilayaBusinessService.GetAll();

            ViewBag.CommuneSeigeSocialId = new SelectList(communes, nameof(Commune.Id), nameof(Commune.DesignationAr));
            ViewBag.WilayaSeigeSocialId = new SelectList(wilayas, nameof(Wilaya.Id), nameof(Wilaya.DisplayNameAr));

            // Pour chaque dropdown de commune, créer la liste appropriée
            if (model != null)
            {
                // Communes pour la wilaya du siège social
                if (model.WilayaSeigeSocialId > 0)
                {
                    var communesResidence = _communeBusinessService.GetAll()
                        .Where(x => x.WilayaId == model.WilayaSeigeSocialId)
                        .ToList();
                    ViewBag.CommuneSeigeSocialId = new SelectList(communesResidence,
                        nameof(Commune.Id), nameof(Commune.DesignationAr),
                        model.CommuneSeigeSocialId);
                }

            }

        }

    }
}
