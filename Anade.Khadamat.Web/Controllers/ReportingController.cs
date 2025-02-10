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
        private readonly ILoadProperties<SessionFormation, int> _loadSessionFormationProperties;
        private readonly IReportService _reportService;
        private readonly InscriptionBusinessService _inscriptionBusinessService;
        private readonly ReferentielBusinessService<Wilaya, int> _wilayaBusinessService;

        private readonly ReferentielBusinessService<Formation, int> _formationBusinessService;

        private readonly SessionFormationBusinessService _sessionFormationBusinessService;

        private readonly DecisionBusinessService _decisionBusinessService;
        private readonly ILoadProperties<Decision, int> _loadDecisionProperties;

        private readonly ReferentielBusinessService<Antenne, int> _antenneBusinessService;
        private readonly ReferentielBusinessService<Agence, int> _agenceBusinessService;


        public ReportingController(ILoadProperties<SessionFormation, int> loadSessionFormationProperties,
                                   IReportService reportService,
                                   InscriptionBusinessService inscriptionBusinessService,
                                   ReferentielBusinessService<Formation, int> formationBusinessService,
                                   ReferentielBusinessService<Wilaya, int> wilayaBusinessService,
                                   SessionFormationBusinessService sessionFormationBusinessService,
                                   DecisionBusinessService decisionBusinessService,
                                   ILoadProperties<Decision, int> loadDecisionProperties,
                                   ReferentielBusinessService<Antenne, int> antenneBusinessService,
                                   ReferentielBusinessService<Agence, int> agenceBusinessService
                                   )

        {
            _loadSessionFormationProperties = loadSessionFormationProperties;
            _inscriptionBusinessService = inscriptionBusinessService;
            _reportService = reportService;
            _formationBusinessService = formationBusinessService;
            _wilayaBusinessService = wilayaBusinessService;
            _sessionFormationBusinessService = sessionFormationBusinessService;
            _decisionBusinessService = decisionBusinessService;
            _loadDecisionProperties = loadDecisionProperties;
            _antenneBusinessService = antenneBusinessService;
            _agenceBusinessService = agenceBusinessService;
        }

        //[Route("{id}")]
        //public IActionResult Print(int id)
        //{

        //    Dictionary<string, string> parameters = new();
        //    Dictionary<string, object> navWithList = new();
        //    string reportPrintName = "";
        //    string reportName = "Attestation";



        //    Type ConventionPropriete = typeof(Inscription);

        //    reportName = "Attestation";

        //    var inscription = _inscriptionBusinessService.GetById(id, _inscriptionBusinessService.GetDefaultLoadProperties());
        //    var wilayas = _wilayaBusinessService.GetAll();

        //    reportPrintName = "AT_" + inscription.NomFr ;

        //    navWithList = _loadInscriptionProperties.GetAllNavProp(inscription);
        //    navWithList.Add("Wilaya", (object)wilayas);

        //    //var formation = _formationBusinessService.GetAllFiltered(x => x.Id == inscription.SessionFormation.FormationId);
        //    //navWithList.Add("Formation", (object)formation);

        //    //var agence = _agenceBusinessService.GetAllFiltered(x => x.Id == inscription.SessionFormation.AgenceId);
        //    //navWithList.Add("Agence", (object)agence);


        //    //navWithList.Add("Baos", fiche.First());


        //    var returnReport = _reportService.GenerateReportAsync(reportName, "pdf", navWithList, parameters);
        //    return File(returnReport, System.Net.Mime.MediaTypeNames.Application.Octet, reportPrintName + ".pdf");

        //}

        [Route("{id}/{SessionId}")]
        public IActionResult PrintAtF(int id,int sessionId)
        {

            Dictionary<string, string> parameters = new();
            Dictionary<string, object> navWithList = new();
            string reportPrintName = "";
            string reportName = "Attestation";
            Type ConventionPropriete = typeof(SessionFormation);

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

        [Route("{id}/{DecisionId}")]
        public IActionResult PrintAtConformitee(int id, int decisionId)
        {
            Dictionary<string, string> parameters = new();
            Dictionary<string, object> navWithList = new();
            string reportPrintName = "";
            string reportName = "ATConformite";
            Type ConventionPropriete = typeof(Decision);
            reportName = "ATConformite";
            var decision = _decisionBusinessService.GetSingle(x => x.InscriptionId == id && x.Id == decisionId, _decisionBusinessService.GetDefaultLoadProperties());
            var wilayas = _wilayaBusinessService.GetAll();
            var antennes = _antenneBusinessService.GetAll();
            var agences = _agenceBusinessService.GetAll();
            reportPrintName = "ATConfirmite_" + decision.Inscription.NomFr;
            navWithList = _loadDecisionProperties.GetAllNavProp(decision);
  
            navWithList.Add("Wilaya", (object)wilayas);
            navWithList.Add("Antenne", (object)antennes);
            navWithList.Add("Agence", (object)agences);

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
