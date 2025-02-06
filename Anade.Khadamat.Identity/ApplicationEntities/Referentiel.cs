using Anade.Domain.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anade.Khadamat.Identity 
{ 
    public abstract class Referentiel : IEntity<int>
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }

        public string Code { get; set; }

        [Display(Name = "Designation (Fr)")]
        [Required(ErrorMessage = "RequiredMessage"), StringLength(200, ErrorMessage = "StringLengthMessage")]
        [Column(TypeName = "varchar(200)")]
        public string Designation { get; set; }

        [Display(Name = "Designation (Ar)")]
        [StringLength(200, ErrorMessage = "StringLengthMessage")]
        [Column(TypeName = "nvarchar(200)")]
        public string DesignationAr { get; set; }
    }
}
