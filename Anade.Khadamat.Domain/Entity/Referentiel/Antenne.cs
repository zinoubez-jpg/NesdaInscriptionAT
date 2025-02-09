using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
   public partial class Antenne : Referentiel
    {
        public Antenne()
        {
            this.Inscriptions = new HashSet<Inscription>();
        }
        public int AgenceId { get; set; }
        public virtual Agence Agence { get; set; }
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    
       
    }
}
