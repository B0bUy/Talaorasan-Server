using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Talaorasan.Server.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}

