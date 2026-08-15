using MotorHub.Application.Common.CurrentUser;
using MotorHub.Application.Common.Mapping;
using MotorHub.Application.Services.Admin;
using MotorHub.Application.Services.Advertisements;
using MotorHub.Application.Services.Authentication;
using MotorHub.Application.Services.Brands;
using MotorHub.Application.Services.Location;
using MotorHub.Application.Services.Models;
using MotorHub.Application.Services.RoleChange;
using Microsoft.Extensions.DependencyInjection;


namespace MotorHub.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IModelService, ModelService>();
            services.AddScoped<IAdvertisementService, AdvertisementService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IRoleChangeService, RoleChangeService>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddMappings();

            return services;
        }
    }
}
