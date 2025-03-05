using Anade.Business.Core;
using Anade.Khadamat.Business;
using Anade.Khadamat.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ReportingController : Controller
    {
        private readonly ILoadProperties<SessionFormationDetails, int> _loadSessionFormationProperties;
        private readonly IReportService _reportService;
        private readonly InscriptionBusinessService _inscriptionBusinessService;
        private readonly ReferentielBusinessService<Wilaya, int> _wilayaBusinessService;

        private readonly ReferentielBusinessService<Formation, int> _formationBusinessService;

        private readonly SessionFormationBusinessService _sessionFormationBusinessService;


        private readonly ReferentielBusinessService<Antenne, int> _antenneBusinessService;
        private readonly ReferentielBusinessService<Agence, int> _agenceBusinessService;
        private readonly ReferentielBusinessService<Commune, int> _communeBusinessService;
        private readonly ReferentielBusinessService<Activite, int> _activiteeBusinessService;

        public ReportingController(ILoadProperties<SessionFormationDetails, int> loadSessionFormationProperties,
                                   IReportService reportService,
                                   InscriptionBusinessService inscriptionBusinessService,
                                   ReferentielBusinessService<Formation, int> formationBusinessService,
                                   ReferentielBusinessService<Wilaya, int> wilayaBusinessService,
                                   SessionFormationBusinessService sessionFormationBusinessService,
                                   ReferentielBusinessService<Antenne, int> antenneBusinessService,
                                   ReferentielBusinessService<Commune, int> communeBusinessService,

                                   ReferentielBusinessService<Agence, int> agenceBusinessService,
                                   ReferentielBusinessService<Activite, int> activiteeBusinessService

            )

        {
            _loadSessionFormationProperties = loadSessionFormationProperties;
            _inscriptionBusinessService = inscriptionBusinessService;
            _reportService = reportService;
            _formationBusinessService = formationBusinessService;
            _wilayaBusinessService = wilayaBusinessService;
            _sessionFormationBusinessService = sessionFormationBusinessService;
            _antenneBusinessService = antenneBusinessService;
            _agenceBusinessService = agenceBusinessService;
            _communeBusinessService = communeBusinessService;
            _activiteeBusinessService = activiteeBusinessService;
        }

   

        [Route("{id}/{SessionId}")]
        public IActionResult PrintAtF(int id,int sessionId)
        {

            Dictionary<string, string> parameters = new();
            Dictionary<string, object> navWithList = new();
            string reportPrintName = "";
            string reportName = "Attestation";
            Type ConventionPropriete = typeof(SessionFormationDetails);

            reportName = "Attestation";

            var sessionFormation = _sessionFormationBusinessService.GetSingle(x =>x.InscriptionId==id && x.Id==sessionId, _sessionFormationBusinessService.GetDefaultLoadProperties());
            var wilayas = _wilayaBusinessService.GetAll();

            reportPrintName = "AT_" + sessionFormation.Inscription.NomFr;

            navWithList = _loadSessionFormationProperties.GetAllNavProp(sessionFormation);
            navWithList.Add("Wilaya", (object)wilayas);

            //var formation = _formationBusinessService.GetAllFiltered(x => x.Id == inscription.SessionFormation.FormationId);
            //navWithList.Add("Formation", (object)formation);

            //var agence = _agenceBusinessService.GetAllFiltered(x => x.Id == inscription.SessionFormation.AgenceId);
            //navWithList.Add("Agence", (object)agence);


            //navWithList.Add("Baos", fiche.First());


            var returnReport = _reportService.GenerateReportAsync(reportName, "pdf", navWithList, parameters);
            return File(returnReport, System.Net.Mime.MediaTypeNames.Application.Octet, reportPrintName + ".pdf");

        }
      
    }
}
