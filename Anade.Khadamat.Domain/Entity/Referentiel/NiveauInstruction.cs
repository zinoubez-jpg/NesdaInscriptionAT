using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class NiveauInstruction : Referentiel
    {
        public NiveauInstruction()
        {
            this.Inscriptions = new HashSet<Inscription>();
        }
        public virtual ICollection<Inscription> Inscriptions { get; set; }
    }
}
