using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class Formation : Referentiel
    {
       
        public virtual ICollection<SessionFormation> SessionFormations { get; set; }
    }
}
