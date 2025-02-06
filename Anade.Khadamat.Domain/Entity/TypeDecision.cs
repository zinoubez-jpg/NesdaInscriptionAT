using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    public partial class TypeDecision : Referentiel
    {
        public TypeDecision()
        {
            this.Decisions = new HashSet<Decision>();
        }
        public virtual ICollection<Decision> Decisions { get; set; }
    }
}
