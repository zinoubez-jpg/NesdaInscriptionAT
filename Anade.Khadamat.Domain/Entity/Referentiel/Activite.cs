using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class Activite : Referentiel
    {
       
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
