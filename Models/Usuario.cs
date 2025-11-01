using Microsoft.AspNetCore.Identity;

namespace Distribuidora.Models
{
    public class Usuario : IdentityUser
    {
        public string? AvatarUrl { get; set; }
    }
}