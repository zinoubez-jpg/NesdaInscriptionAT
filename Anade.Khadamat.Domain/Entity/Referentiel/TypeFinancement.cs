using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
  

    public partial class TypeFinancement : Referentiel
    {
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
