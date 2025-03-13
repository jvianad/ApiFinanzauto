using Microsoft.AspNetCore.Identity;

namespace ApiFinanzauto.Models
{
    public class UserApp : IdentityUser
    {
        public string? FullName { get; set; }
    }
}