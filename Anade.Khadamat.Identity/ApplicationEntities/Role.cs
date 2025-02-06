using Anade.Domain.Core;
using Microsoft.AspNetCore.Identity;

namespace Anade.Khadamat.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Role : IdentityRole, IEntity<string>
    {

    }
}
