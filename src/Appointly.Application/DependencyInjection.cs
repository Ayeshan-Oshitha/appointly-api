using Appointly.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;


namespace Appointly.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
