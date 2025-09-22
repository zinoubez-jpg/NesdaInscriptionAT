using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class CentreDeveloppementEntrepreneuriat : Referentiel
    {
        public CentreDeveloppementEntrepreneuriat()
        {
            this.Inscriptions = new HashSet<Inscription>();
        }

        public int WilayaId { get; set; }
        public int TypeCentreDeveloppementEntrepreneuriatId { get; set; }

        public virtual Wilaya Wilaya { get; set; }
        public virtual TypeCentreDeveloppementEntrepreneuriat TypeCentreDeveloppementEntrepreneuriat { get; set; }
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
