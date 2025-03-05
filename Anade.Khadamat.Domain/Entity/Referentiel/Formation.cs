using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class Formation : Referentiel
    {
       
        public virtual ICollection<SessionFormationDetails> SessionFormations { get; set; }
    }
}
