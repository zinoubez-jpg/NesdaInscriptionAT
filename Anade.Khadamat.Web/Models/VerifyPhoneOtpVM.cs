using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Web.Models
{
    public class VerifyPhoneOtpVM
    {
        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "الرجاء إدخال عنوان بريد إلكتروني صحيح.")]
        public string Email { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف.")]
        [RegularExpression(@"^0\s?[567].*$", ErrorMessage = "يرجى التحقق من رقم الهاتف الذي أدخلته")]
        public string NumTelephone { get; set; }

        [Display(Name = "رمز التحقق")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "رمز التحقق غير صحيح.")]
        [RegularExpression(@"^\d{4,10}$", ErrorMessage = "يجب أن يتكون الرمز من أرقام فقط.")]
        public string OtpCode { get; set; }

        /// <summary>
        /// false = enter phone / request SMS ; true = enter OTP.
        /// </summary>
        public bool OtpStep { get; set; }

        public string MaskedPhone { get; set; }
        public int ValidityMinutes { get; set; }
        public int RemainingSeconds { get; set; }
    }
}
