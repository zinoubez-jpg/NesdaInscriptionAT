using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class Wilaya : Referentiel
    {
        public Wilaya()
        {
            this.Communes = new HashSet<Commune>();
            this.CentresDeveloppementEntrepreneuriat = new HashSet<CentreDeveloppementEntrepreneuriat>();
        }

        public virtual ICollection<Commune> Communes { get; set; }
        public virtual ICollection<CentreDeveloppementEntrepreneuriat> CentresDeveloppementEntrepreneuriat { get; set; }
    }
}
