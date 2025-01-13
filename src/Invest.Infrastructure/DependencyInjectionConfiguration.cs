using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Interfaces.Services;
using Template.Domain.Services;

namespace Template.Infrastructure
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<TemplateDbContext>();
            services.AddScoped<SupabaseService>();

            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
