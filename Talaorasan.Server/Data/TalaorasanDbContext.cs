using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Talaorasan.Server.Entities;

namespace Talaorasan.Server.Data
{
    public class TalaorasanDbContext
     : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public TalaorasanDbContext(DbContextOptions<TalaorasanDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons => Set<Person>();
        public DbSet<FaceTemplate> FaceTemplates => Set<FaceTemplate>();
        public DbSet<EnrollmentImage> EnrollmentImages => Set<EnrollmentImage>();
        public DbSet<Device> Devices => Set<Device>();
        //public DbSet<DeviceRegistrationRequest> DeviceRegistrationRequests => Set<DeviceRegistrationRequest>();
        public DbSet<AttendanceLog> AttendanceLogs => Set<AttendanceLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FaceTemplate>()
                .HasIndex(x => new { x.PersonId, x.IsActive });

            builder.Entity<AttendanceLog>()
                .HasIndex(x => new { x.DeviceId, x.CapturedUtc });
        }
    }
}
