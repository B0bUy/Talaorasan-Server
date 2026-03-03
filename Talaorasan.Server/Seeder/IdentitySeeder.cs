using Microsoft.AspNetCore.Identity;
using Talaorasan.Server.Entities;

namespace Talaorasan.Server.Seeder
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roles = { "SuperAdmin", "Admin", "HR", "Viewer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        CreatedUtc = DateTime.UtcNow
                    });
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var users = new List<(string Email, string firstname, string lastname, string Role)>
        {
            ("superadmin@talaorasan.local", "Super", "Administrator", "SuperAdmin"),
            ("admin@talaorasan.local", "System", "Administrator", "Admin"),
            ("hr@talaorasan.local", "HR", "Officer", "HR"),
            ("viewer@talaorasan.local", "Viewer", "Account", "Viewer")
        };

            const string defaultPassword = "Talaorasan@123";

            foreach (var (email, firstname, lastname, role) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(email);

                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = email,
                        Email = email,
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        EmailConfirmed = true,
                        FirstName = firstname,
                        LastName = lastname,
                        IsActive = true,
                        CreatedUtc = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);

                    if (!result.Succeeded)
                    {
                        throw new Exception("User seed failed: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }

                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
