using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class TypeCentreDeveloppementEntrepreneuriat : Referentiel
    {
        public TypeCentreDeveloppementEntrepreneuriat()
        {
            this.CentresDeveloppementEntrepreneuriat = new HashSet<CentreDeveloppementEntrepreneuriat>();
        }

        public virtual ICollection<CentreDeveloppementEntrepreneuriat> CentresDeveloppementEntrepreneuriat { get; set; }
    }
}
