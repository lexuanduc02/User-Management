using App.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace App.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
        }

        public DbConnection GetDbConnect()
        {
            return base.Database.GetDbConnection();
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
