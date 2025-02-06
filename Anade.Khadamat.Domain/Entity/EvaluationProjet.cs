using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    public partial class EvaluationProjet : IEntity<int>
    {
        public int Id { get; set; }
        public string CodeFiche { get; set; }
        public string ScoreTotal { get; set; }
        public bool IsAdmis { get; set; }
        public DateTime DateEvaluation { get; set; }
        public int InscriptionId { get; set; }
    }

}

