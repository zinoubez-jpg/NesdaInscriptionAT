using System.Collections.Generic;

namespace Anade.Khadamat.Identity
{
    public class Niveau : Referentiel
    {
        public virtual ICollection<Structure> Structures { get; set; }

    }
}
