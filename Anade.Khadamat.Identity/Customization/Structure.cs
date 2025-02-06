using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Identity
{
  public partial  class Structure
    {
        [NotMapped]
        public string NameStructure { get => $"({CodeStructure}) {Designation}"; }
    }
}
