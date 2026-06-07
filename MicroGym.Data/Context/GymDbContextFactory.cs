using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MicroGym.Data.Context
{
    public class GymDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
    {
        public GymDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\Phol;Database=GymDB;Trusted_Connection=True;TrustServerCertificate=True;");
            return new GymDbContext(optionsBuilder.Options);
        }
    }
}
