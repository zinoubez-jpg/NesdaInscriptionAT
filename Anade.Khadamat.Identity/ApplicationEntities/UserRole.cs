using Anade.Domain.Core;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anade.Khadamat.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class UserRole : IdentityUserRole<string>, IEntity<int>
    {
        [NotMapped]
        public int Id { get; set; }
    }
}
