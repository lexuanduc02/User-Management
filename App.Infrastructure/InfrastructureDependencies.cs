using App.Application.Contracts.Infrastructure.Repositories;
using App.Application.Contracts.Infrastructure.UnitOfWork;
using App.Infrastructure.Context;
using App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("=======Has not define connection string yet!!!=======");
            }

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services
                .AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
