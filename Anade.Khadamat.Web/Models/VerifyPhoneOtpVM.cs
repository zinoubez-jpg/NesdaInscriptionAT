using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.Models
{
    public class VerifyPhoneOtpVM
    {
        public int InscriptionId { get; set; }

        [Display(Name = "Téléphone")]
        public string MaskedPhone { get; set; }

        [Display(Name = "Code OTP")]
        [Required(ErrorMessage = "Veuillez saisir le code reçu par SMS.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Le code OTP est invalide.")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "Le code OTP doit contenir uniquement des chiffres.")]
        public string OtpCode { get; set; }

        public int ValidityMinutes { get; set; }
    }

    public class ResendPhoneOtpVM
    {
        [Display(Name = "Numéro de téléphone")]
        [Required(ErrorMessage = "Veuillez saisir votre numéro de téléphone.")]
        [RegularExpression(@"^0\s?[567].*$", ErrorMessage = "يرجى التحقق من رقم الهاتف الذي أدخلته")]
        public string NumTelephone { get; set; }
    }
}
