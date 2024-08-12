using App.Application.Services;
using App.Application.Services.Contracts;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Application
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
