using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Services.Admin;
using Appointly.Application.Services.Advertisements;
using Appointly.Application.Services.Advertisments;
using Appointly.Application.Services.Authentication;
using Appointly.Application.Services.Brands;
using Appointly.Application.Services.Location;
using Appointly.Application.Services.Models;
using Appointly.Application.Services.RoleChange;
using Microsoft.Extensions.DependencyInjection;


namespace Appointly.Application
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

            return services;
        }
    }
}
