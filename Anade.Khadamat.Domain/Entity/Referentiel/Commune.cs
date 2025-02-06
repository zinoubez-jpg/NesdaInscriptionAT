using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Domain
{
    public partial class Commune : Referentiel
    {

        [Display(Name = "WilayaId")]
        [Required(ErrorMessage = "RequiredMessage")]
        public int WilayaId { get; set; }
        public virtual Wilaya Wilaya { get; set; }
        public virtual ICollection<Inscription> CommuneResidences { get; set; }
        public virtual ICollection<Inscription> CommuneNaissances { get; set; }
        public virtual ICollection<Inscription> CommuneSeigeSocials { get; set; }
    }
}
