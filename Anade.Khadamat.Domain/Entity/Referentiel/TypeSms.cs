using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class TypeSms : Referentiel
    {
        public TypeSms()
        {
            this.SmsSents = new HashSet<SmsSent>();
        }

        public virtual ICollection<SmsSent> SmsSents { get; set; }
    }
}
