using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Talaorasan.Server.Data
{
    public sealed class TalaorasanDbContextFactory : IDesignTimeDbContextFactory<TalaorasanDbContext>
    {
        public TalaorasanDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TalaorasanDbContext>();

            // 1) Prefer env var (safe for CLI + CI)
            var cs = Environment.GetEnvironmentVariable("TALAORASAN_CONNECTION")
                     ?? "server=localhost;port=3307;database=talaorasan;user=root;password=pass;";

            optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));

            return new TalaorasanDbContext(optionsBuilder.Options);
        }
    }
}
