using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public string Id { get; set; }
        [Display(Name = "Nom")]
        [Required(ErrorMessage = "RequiredMessage"), StringLength(256, ErrorMessage = "StringLengthMessage")]
        public string Nom { get; set; }
        [Display(Name = "Prenom")]
        [Required(ErrorMessage = "RequiredMessage"), StringLength(256, ErrorMessage = "StringLengthMessage")]
        public string Prenom { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int TypeUserId { get; set; }
        public int StructureId { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
