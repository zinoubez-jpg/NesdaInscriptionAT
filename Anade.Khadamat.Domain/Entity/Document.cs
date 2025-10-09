using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{

    public partial class Document : IEntity<int>
    {
        public int Id { get; set; }
        public string FilePath { get; set; }

        public int TypeDocumentId { get; set; }
        public int InscriptionId { get; set; }
        public virtual TypeDocument TypeDocument { get; set; }
        public virtual Inscription Inscription { get; set; }
    }
}
