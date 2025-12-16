using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Infrastructure.Persistence.Repositories;
using Appointly.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appointly.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IuserRepository, UserRepository>();
            return services;
        }
    }
}
