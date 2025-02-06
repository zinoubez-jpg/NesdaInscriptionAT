using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class TypeInscription : Referentiel
    {
        public TypeInscription()
        {
            this.Inscriptions = new HashSet<Inscription>();
        }
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
