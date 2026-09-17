using Anade.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anade.Khadamat.Domain
 {
    public partial class Inscription : IEntity<int>
    {
        public Inscription()
        {
            this.Documents = new HashSet<Document>();
            this.SmsSents = new HashSet<SmsSent>();
        }
         
        [Display(Name = "Id")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int Id { get; set; }
        public int TypeInscriptionId { get; set; }
        public int CategorieId { get; set; }
        public DateTime DateInscription { get; set; }

        [Display(Name = "NIN")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(18, ErrorMessage = "StringLengthMessage")]
        [RegularExpression(@"^\d{18}$", ErrorMessage = "يجب أن يتكون الرقم التعريفي الوطني الوحيد من 18 رقم.")]
        public string NIN { get; set; }
        [Display(Name = "NomFr")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(128, ErrorMessage = "StringLengthMessage")]
        public string NomFr { get; set; }
        [Display(Name = "PrenomFr")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(128, ErrorMessage = "StringLengthMessage")]
        public string PrenomFr { get; set; }
        [Display(Name = "NomAr")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(128, ErrorMessage = "StringLengthMessage")]
        public string NomAr { get; set; }
        [Display(Name = "PrenomAr")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(128, ErrorMessage = "StringLengthMessage")]
        public string PrenomAr { get; set; }
        [Display(Name = "GenreId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int GenreId { get; set; }
        [Display(Name = "DateNaissance"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public DateTime DateNaissance { get; set; }

        [Display(Name = "WilayaNaissanceId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int WilayaNaissanceId { get; set; }
        [Display(Name = "CommuneNaissanceId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int CommuneNaissanceId { get; set; }

        [Display(Name = "SituationFamilialeId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int SituationFamilialeId { get; set; }
        [Display(Name = "NomJeunneFille"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string NomJeunneFille { get; set; }
        [Display(Name = "NomJeunneFilleAr"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string NomJeunneFilleAr { get; set; }
        [Display(Name = "AdressAr")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل"), StringLength(128, ErrorMessage = "StringLengthMessage")]
        public string AdressAr { get; set; }
        public string AdressFr { get; set; }
        [Display(Name = "WilayaResidenceId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int WilayaResidenceId { get; set; }
        [Display(Name = "CommuneResidenceId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int CommuneResidenceId { get; set; }
        [Display(Name = "NumTelephone")]
        [RegularExpression(@"^0\s[567].*$", ErrorMessage = "يرجى التحقق من رقم الهاتف الذي أدخلته")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public string NumTelephone { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "الرجاء إدخال عنوان بريد إلكتروني صحيح.")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public string Email { get; set; }
        [Display(Name = "NiveauInstructionId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int NiveauInstructionId { get; set; }
        [Display(Name = "TypeFinancementId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int TypeFinancementId { get; set; }

        public string EtablissementFormation { get; set; }

        public string IntituleDiplome { get; set; }

        public string Specialite { get; set; }

        [Display(Name = "WilayaSeigeSocialId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int WilayaSeigeSocialId { get; set; }
        [Display(Name = "CommuneSeigeSocialId")]
        [Required(ErrorMessage = "يجب ملء هذا الحقل")]
        public int CommuneSeigeSocialId { get; set; }
        public string AddresseArSeigeSocial { get; set; }
        public string AddresseFrSeigeSocial { get; set; }
        public string RaisonSociale { get; set; }
        public string RaisonSocialeAr { get; set; }

        public int AntenneId { get; set; }
        public string Anntenne { get; set; }
        public string Agence { get; set; }
        public bool Validee { get; set; }
        public string ValideePAR { get; set; }
        public DateTime DateValidation { get; set; }
        public bool Annulee { get; set; }
        public string AnnuleePAR { get; set; }
        public DateTime DateAnnulation { get; set; }
        public string MotifAnnulation { get; set; }

        public bool IsEmailVerified { get; set; }
        public string EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }

        [StringLength(10, ErrorMessage = "StringLengthMessage")]
        public string OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsPhoneValidated { get; set; }

        [Required(ErrorMessage = "يرجى التحقق من الكابتشا")]
        [Display(Name = "ReCaptcha")]
        public string RecaptchaResponse { get; set; }

        public string NumeroSiejeV3 { get; set; }
        public int ActiviteId { get; set; }
        public string PasswordHashInit { get; set; }

        public virtual ICollection<SessionFormationDetails> SessionFormations { get; set; }
        public virtual ICollection<EvaluationProjet> EvaluationProjets { get; set; }
        public virtual ICollection<CSVF> CSVFs { get; set; }
        public virtual Commune CommuneNaissance { get; set; }
        public virtual Commune CommuneResidence { get; set; }
        public virtual Commune CommuneSiegeSocial { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual SituationFamiliale SituationFamiliale { get; set; }
        public virtual NiveauInstruction NiveauInstruction { get; set; }
        public virtual TypeInscription TypeInscription { get; set; }
        public virtual Antenne Antenne { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual Activite Activite { get; set; }
        public virtual TypeFinancement TypeFinancement { get; set; }

        public int CentreDeveloppementEntrepreneuriatId { get; set; }
        public virtual CentreDeveloppementEntrepreneuriat CentreDeveloppementEntrepreneuriat { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<SmsSent> SmsSents { get; set; }
    }
}
