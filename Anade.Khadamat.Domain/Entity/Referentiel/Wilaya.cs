using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class Wilaya : Referentiel
    {
        public Wilaya()
        {
            this.Communes = new HashSet<Commune>();
        }

        public virtual ICollection<Commune> Communes { get; set; }
    }
}
