using Anade.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Domain
{
    public class SmsSent : IEntity<int>
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "NumTelephone")]
        [Required(ErrorMessage = "Ce champ est obligatoire"), StringLength(20, ErrorMessage = "StringLengthMessage")]
        public string NumTelephone { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "Ce champ est obligatoire"), StringLength(500, ErrorMessage = "StringLengthMessage")]
        public string Message { get; set; }

        [Display(Name = "TypeSmsId")]
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public int TypeSmsId { get; set; }

        public int? InscriptionId { get; set; }

        [Display(Name = "DateEnvoi")]
        public DateTime DateEnvoi { get; set; }

        [Display(Name = "StatutEnvoiSmsId")]
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public int StatutEnvoiSmsId { get; set; }

        [Display(Name = "ReponseApi")]
        [StringLength(2000, ErrorMessage = "StringLengthMessage")]
        public string ReponseApi { get; set; }

        [Display(Name = "DateReponse")]
        public DateTime? DateReponse { get; set; }

        public virtual TypeSms TypeSms { get; set; }
        public virtual StatutEnvoiSms StatutEnvoiSms { get; set; }
        public virtual Inscription Inscription { get; set; }
    }
}
