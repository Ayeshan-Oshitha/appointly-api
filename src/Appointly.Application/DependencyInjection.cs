using Appointly.Application.Services.Authentication;
using Appointly.Application.Services.Location;
using Microsoft.Extensions.DependencyInjection;


namespace Appointly.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILocationService, LocationService>();
            return services;
        }
    }
}
