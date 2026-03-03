using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talaorasan.Server.Entities;
using Talaorasan.Shared.Requests;
using Talaorasan.Shared.Response;

namespace Talaorasan.Server.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
        {
            var authGroup = builder.MapGroup("/api/auth")
                                   .WithTags("Authentication");
            authGroup.MapPost("/login", async (
            LoginRequestDto request,
            UserManager<ApplicationUser> userManager,
            IConfiguration config) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null || !user.IsActive)
                    return Results.Unauthorized();

                var valid = await userManager.CheckPasswordAsync(user, request.Password);
                if (!valid)
                    return Results.Unauthorized();

                var roles = await userManager.GetRolesAsync(user);

                var jwtSection = config.GetSection("Jwt");
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSection["Key"]!)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expires = DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSection["AccessTokenMinutes"] ?? "60")
                );

                var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new("firstName", user.FirstName),
                new("lastName", user.LastName)
            };

                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

                var token = new JwtSecurityToken(
                    issuer: jwtSection["Issuer"],
                    audience: jwtSection["Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new LoginResponseDto(
                    tokenStr,
                    expires,
                    user.UserName,
                    roles.ToArray()
                ));
            });
            return authGroup;
        }
    }
}
