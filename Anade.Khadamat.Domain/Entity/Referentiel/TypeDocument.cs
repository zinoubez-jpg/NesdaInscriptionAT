using System.Collections.Generic;

namespace Anade.Khadamat.Domain
{
    public partial class TypeDocument : Referentiel
    {
        public TypeDocument()
        {
            this.Documents = new HashSet<Document>();
        }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
