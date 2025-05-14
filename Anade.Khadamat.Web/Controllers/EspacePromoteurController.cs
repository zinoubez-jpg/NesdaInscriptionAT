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
                                          ReferentielBusinessService<Formation, int> formationBusinessService
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

        }
        public IActionResult DetailsPromoteur()
        {
            var user =  _userService.GetUserEagerLoadedAsync(User).Result;
            var inscrit = _inscriptionBusinessService.GetSingle(x => x.Email == user.Email,_inscriptionBusinessService.GetDefaultLoadProperties());
            var WilayaNais = _wilayaBusinessService.GetById(inscrit.CommuneNaissance.WilayaId);
            var wilayaRes= _wilayaBusinessService.GetById(inscrit.CommuneResidence.WilayaId);
            var wilayaSS = _wilayaBusinessService.GetById(inscrit.CommuneSiegeSocial.WilayaId);
           
            inscrit.CommuneNaissance.Wilaya = WilayaNais;
            inscrit.CommuneResidence.Wilaya = wilayaRes;
            inscrit.CommuneSiegeSocial.Wilaya = wilayaSS;
            foreach (var item in inscrit.SessionFormations)
            {
                item.Formation = _formationBusinessService.GetById(item.FormationId);
            }

            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }



            return View(inscrit);
        }

        public IActionResult SuiviDossier()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;
            var inscrit = _inscriptionBusinessService.GetSingle(x => x.Email == user.Email && x.IsEmailVerified, _inscriptionBusinessService.GetDefaultLoadProperties());
            var WilayaNais = _wilayaBusinessService.GetById(inscrit.CommuneNaissance.WilayaId);
            var wilayaRes = _wilayaBusinessService.GetById(inscrit.CommuneResidence.WilayaId);
            var wilayaSS = _wilayaBusinessService.GetById(inscrit.CommuneSiegeSocial.WilayaId);
            inscrit.CommuneNaissance.Wilaya = WilayaNais;
            inscrit.CommuneResidence.Wilaya = wilayaRes;
            inscrit.CommuneSiegeSocial.Wilaya = wilayaSS;
            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }

            return View(inscrit);
        }

        public IActionResult ATFormation()
        {
            var user = _userService.GetUserEagerLoadedAsync(User).Result;
            var inscrit = _inscriptionBusinessService.GetSingle(x => x.Email == user.Email, _inscriptionBusinessService.GetDefaultLoadProperties());
            foreach (var item in inscrit.SessionFormations)
            {
                item.Formation = _formationBusinessService.GetById(item.FormationId);
            }
            if (inscrit == null)
            {
                return NotFound("Inscription non trouvée ou email non vérifié.");
            }

            if (inscrit.Annulee)
            {
                return Content("Votre inscription est annulée.");
            }

            return View(inscrit);
        }
        public IActionResult SimulBPlan()
        {
            

            return View();
        }
        public IActionResult SimulVAN()
        {


            return View();
        }

    }
}
