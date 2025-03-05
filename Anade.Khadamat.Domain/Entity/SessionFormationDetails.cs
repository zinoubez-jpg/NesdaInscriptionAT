using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    public partial class SessionFormationDetails : IEntity<int>
    {
        public int Id { get; set; }
        public string CodeSession { get; set; }
        public int FormationId { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public bool IsFormee { get; set; }
        public int InscriptionId { get; set; }
        public virtual Inscription Inscription { get; set; }
        public virtual Formation Formation { get; set; }

    }
}
