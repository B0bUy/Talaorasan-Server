using Microsoft.AspNetCore.Identity;

namespace Talaorasan.Server.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
