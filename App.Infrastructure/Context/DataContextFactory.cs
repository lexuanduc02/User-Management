using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace App.Infrastructure.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string connectionString = "Server=.;Database=UserManagement_LXD;Trusted_Connection=True;TrustServerCertificate=True;";
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}
