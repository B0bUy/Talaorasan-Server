using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Talaorasan.Server.Data;
using Talaorasan.Server.Entities;
using Talaorasan.Server.Seeder;
using Talaorasan.Server.Endpoints;
using Talaorasan.Server.Logic;
using Talaorasan.Server;

var builder = WebApplication.CreateBuilder(args);

// =========================
// DATABASE (MySQL - Pomelo)
// =========================
builder.Services.AddDbContext<TalaorasanDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("Default")
             ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");

    options.UseMySql(cs, ServerVersion.AutoDetect(cs));
});

// =========================
// IDENTITY
// =========================
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<TalaorasanDbContext>()
    .AddDefaultTokenProviders();

// =========================
// JWT AUTHENTICATION
// =========================
var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSection["Key"]!;
var issuer = jwtSection["Issuer"]!;
var audience = jwtSection["Audience"]!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        // Enable JWT for SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs/attendance"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

// =========================
// AUTHORIZATION POLICIES
// =========================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireSuperAdmin",
        p => p.RequireRole("SuperAdmin"));

    options.AddPolicy("RequireAdmin",
        p => p.RequireRole("Admin", "SuperAdmin"));

    options.AddPolicy("RequireHR",
        p => p.RequireRole("HR", "Admin", "SuperAdmin"));

    options.AddPolicy("RequireViewer",
        p => p.RequireRole("Viewer", "HR", "Admin", "SuperAdmin"));
});

// =========================
// SERVICES
// =========================
builder.Services.AddSignalR();
builder.Services.AddOpenApi(); // .NET 9 OpenAPI (for Scalar)

builder.Services.AddTalaorasanServer(builder.Configuration);

var app = builder.Build();

// =========================
// APPLY MIGRATIONS + SEED
// =========================
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TalaorasanDbContext>();
    await db.Database.MigrateAsync();

    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

// =========================
// DEVELOPMENT TOOLS
// =========================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();            // OpenAPI JSON
    app.MapScalarApiReference(); // Scalar UI
}

// =========================
// MIDDLEWARE PIPELINE
// =========================
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// =========================
// ENDPOINT MAPPING (Minimal API)
// =========================
app.MapAuthEndpoints();  // Your Minimal API auth mapping
app.MapPersonEndpoints();  // Your Minimal API auth mapping
// app.MapHub<AttendanceHub>("/hubs/attendance");

app.Run();