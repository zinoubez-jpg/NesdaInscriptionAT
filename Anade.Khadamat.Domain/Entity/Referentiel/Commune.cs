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
        public virtual ICollection<Inscription> InscriptionsPCRes { get; set; }
        public virtual ICollection<Inscription> InscriptionsPCNais { get; set; }
        public virtual ICollection<Inscription> InscriptionsPCSS { get; set; }
    }
}
