using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class StatutEnvoiSms : Referentiel
    {
        public StatutEnvoiSms()
        {
            this.SmsSents = new HashSet<SmsSent>();
        }

        public virtual ICollection<SmsSent> SmsSents { get; set; }
    }
}
