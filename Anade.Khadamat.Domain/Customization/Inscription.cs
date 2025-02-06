using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
   public partial class Inscription
    {
        [NotMapped]
        public string FullName { get => $"{NomFr} {PrenomFr}"; }
        [NotMapped]
        public string FullNameAr { get => $"{NomAr} {PrenomAr}"; }

    }
}
