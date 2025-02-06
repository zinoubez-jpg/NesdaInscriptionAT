using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anade.Khadamat.Domain
{
    public abstract partial class Referentiel : IEntity<int>
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "RequiredMessage")]
        public int Id { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "RequiredMessage"), StringLength(10, ErrorMessage = "StringLengthMessage")]
        public string Code { get; set; }

        [Display(Name = "DesignationFr")]
        [Required(ErrorMessage = "RequiredMessage"), StringLength(256, ErrorMessage = "StringLengthMessage")]
        public string DesignationFr { get; set; }

        [Display(Name = "DesignationAr")]
        [StringLength(256, ErrorMessage = "StringLengthMessage")]
        public string DesignationAr { get; set; }
    }
}
