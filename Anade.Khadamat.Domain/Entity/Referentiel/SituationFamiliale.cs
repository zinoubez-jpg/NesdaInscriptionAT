using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class SituationFamiliale : Referentiel
    {
        public SituationFamiliale()
        {
            this.Inscriptions = new HashSet<Inscription>();
        }
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }

}
