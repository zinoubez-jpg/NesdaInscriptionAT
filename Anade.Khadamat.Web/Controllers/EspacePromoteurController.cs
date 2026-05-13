using Anade.Khadamat.Business;
using Anade.Khadamat.Domain;
using Anade.Khadamat.Identity;
using Anade.Khadamat.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly UserStructureService _userStructureService;
        private readonly TypeUserServices _typeUserServices;
        private readonly StructureService _structureService;

        private readonly PromoteurInscriptionBusinesService _promoteurInscriptionBusinessService;
        private readonly Detail_DossierInscriptionBusinesService _detail_DossierInscriptionBusinessService;
        private readonly AssociesInscriptionBusinesService _associesInscriptionBusinessService;

        protected readonly CompteAuthentificationBusinesService _compteAuthentificationBusinesService;
        protected readonly Identifiant_SIAuthentificationBusinesService _identifiant_SIAuthentificationBusinesService;
        protected readonly Suivi_Accepte_CSVFBusinesService _suivi_Accepte_CSVFBusinesService;
        protected readonly Suivi_EvaluationBusinesService _suivi_EvaluationBusinesService;
        protected readonly Suivi_FinanceBusinesService _suivi_FinanceBusinesService;
        protected readonly Suivi_FormationBusinesService _suivi_FormationBusinesService;
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
                                          PromoteurInscriptionBusinesService promoteurInscriptionBusinessService,
                                          Detail_DossierInscriptionBusinesService detail_DossierInscriptionBusinessService,
                                          AssociesInscriptionBusinesService associesInscriptionBusinessService,
                                          CompteAuthentificationBusinesService compteAuthentificationBusinesService,
                                          Identifiant_SIAuthentificationBusinesService identifiant_SIAuthentificationBusinesService,
                                          Suivi_Accepte_CSVFBusinesService suivi_Accepte_CSVFBusinesService,
                                          Suivi_EvaluationBusinesService suivi_EvaluationBusinesService,
                                          Suivi_FinanceBusinesService suivi_FinanceBusinesService,
                                          Suivi_FormationBusinesService suivi_FormationBusinesService
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
            _roleManager = roleManager;
            _userManager = userManager;
            _userService = userService;
            _userStructureService = userStructureService;
            _structureService = structureService;
            _promoteurInscriptionBusinessService = promoteurInscriptionBusinessService;
            _detail_DossierInscriptionBusinessService = detail_DossierInscriptionBusinessService;
            _associesInscriptionBusinessService = associesInscriptionBusinessService;
            _compteAuthentificationBusinesService = compteAuthentificationBusinesService;
            _identifiant_SIAuthentificationBusinesService = identifiant_SIAuthentificationBusinesService;
            _suivi_Accepte_CSVFBusinesService = suivi_Accepte_CSVFBusinesService;
            _suivi_EvaluationBusinesService = suivi_EvaluationBusinesService;
            _suivi_FinanceBusinesService = suivi_FinanceBusinesService;
            _suivi_FormationBusinesService = suivi_FormationBusinesService;
        }
        public IActionResult DetailsPromoteur()
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

            var WilayaNais = _wilayaBusinessService.GetById(inscrit.CommuneNaissance.WilayaId);
            var wilayaRes = _wilayaBusinessService.GetById(inscrit.CommuneResidence.WilayaId);
            var wilayaSS = _wilayaBusinessService.GetById(inscrit.CommuneSiegeSocial.WilayaId);

            inscrit.CommuneNaissance.Wilaya = WilayaNais;
            inscrit.CommuneResidence.Wilaya = wilayaRes;
            inscrit.CommuneSiegeSocial.Wilaya = wilayaSS;

            foreach (var item in inscrit.SessionFormations)
            {
                item.Formation = _formationBusinessService.GetById(item.FormationId);
            }

            return View(inscrit);
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


            if (inscrit.TypeInscriptionId == TypesInscriptionId.Extension)
            {
                return View("SuiviDossier", inscrit);
            }
            else
            {
                var inscriptionSuiviCreation = new InscriptionSuiviCreation(inscrit);

                var p = _promoteurInscriptionBusinessService.GetByNIN(inscriptionSuiviCreation.NIN);
                var c = _compteAuthentificationBusinesService.GetByUserName(inscriptionSuiviCreation.Email);
                var i = _identifiant_SIAuthentificationBusinesService.GetSingle(x => x.Authentification_CompteId == c.Id);

                if (c.Date_Validation != null)
                {
                    inscriptionSuiviCreation.DateValidation = (DateTime)c.Date_Validation;
                }


                var csvf = _suivi_Accepte_CSVFBusinesService.GetSingle(x => x.Num_Doss == i.Num_SI);
                var eval = _suivi_EvaluationBusinesService.GetSingle(x => x.PromoteurId == i.Num_SI);
                var finc = _suivi_FinanceBusinesService.GetSingle(x => x.Num_Doss == i.Num_SI);

                var form = _suivi_FormationBusinesService.GetAllFiltered(x => x.NIN == p.NIN)
                                                         .OrderByDescending(x => x.DateDebutSessionFormation)
                                                         .FirstOrDefault();

                
                inscriptionSuiviCreation.Date_csvf = csvf.Date_csvf;
                inscriptionSuiviCreation.ScoreTotal = eval.ScoreTotal.ToString();
                inscriptionSuiviCreation.Date_Finance = finc.Date_Finance;
                inscriptionSuiviCreation.DateDebutSessionFormation = form.DateDebutSessionFormation;

                return View("SuiviDossierCreation", inscriptionSuiviCreation);
            }
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
        //public IActionResult SimulBPlan()
        //{


        //    return View();
        //}
        //public IActionResult SimulVAN()
        //{


        //    return View();
        //}

    }
}
