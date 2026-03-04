using Microsoft.EntityFrameworkCore;
using Talaorasan.Server.Data;
using Talaorasan.Server.Logic;

namespace Talaorasan.Server
{
    public static class RegisterServices
    {
        public static IServiceCollection AddTalaorasanServer(this IServiceCollection services, IConfiguration config)
        {
            // DbContext
            services.AddDbContext<TalaorasanDbContext>(options =>
            {
                var cs = config.GetConnectionString("Default")
                         ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
                options.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });

            // Your app services (examples)
            services.AddScoped<FileManagementService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            return services;
        }
    }
}
