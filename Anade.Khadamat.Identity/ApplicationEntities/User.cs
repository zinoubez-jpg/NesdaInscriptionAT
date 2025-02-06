using Anade.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Anade.Khadamat.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public partial class User : IdentityUser, IEntity<string>
    {
        public User()
        {
            Structures = new HashSet<UserStructure>();
        }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateCreation { get; set; }
        public int? OrganismeId { get; set; }
        public int TypeUserId { get; set; }
        public virtual TypeUser TypeUser { get; set; } //Type du compte "Anade/Projet/".
        public virtual ICollection<UserStructure> Structures { get; set; }


    }
}
