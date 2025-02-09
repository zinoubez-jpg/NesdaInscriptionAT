using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{

    public partial class CSVF : IEntity<int>
    {
        public int Id { get; set; }
        public string CodeSession { get; set; }
        public string ResultatCSVF { get; set; }
        public DateTime DateCSVF { get; set; }
        public bool IsValide { get; set; }
        public int InscriptionId { get; set; }


    }
}
