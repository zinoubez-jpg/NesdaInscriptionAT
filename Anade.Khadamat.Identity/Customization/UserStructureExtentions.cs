using System.ComponentModel.DataAnnotations.Schema;

namespace Anade.Khadamat.Identity
{
    public partial class User
    {
        [NotMapped]
        public string FullName { get => $"{Nom} {Prenom}"; }

    }
}
