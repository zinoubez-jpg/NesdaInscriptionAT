using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
  
    public partial class Circonscription : IEntity<int>
    {
        public int Id { get; set; }
        public string CodeAntenne { get; set; }
        public string CodeCommune { get; set; }

    }
}
