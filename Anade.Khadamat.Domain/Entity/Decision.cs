using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
   
    public partial class Decision : IEntity<int>
    {
        public int Id { get; set; }
        public string CodeDecision { get; set; }
        public int TypeDecisionId { get; set; }
        public bool IsEligible { get; set; }
        public DateTime DateEtablissementAE { get; set; }
        public int InscriptionId { get; set; }

        public virtual TypeDecision TypeDecision { get; set; }
        public virtual Inscription Inscription { get; set; }

    }
}
