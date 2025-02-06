using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    public partial class Wilaya
    {
        [NotMapped]
        public string DisplayName { get => $"({Code}) {DesignationFr}"; }
        [NotMapped]
        public string DisplayNameAr { get => $"({Code}) {DesignationAr}"; }
    }
}
