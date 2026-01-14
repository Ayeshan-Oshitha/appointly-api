using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Infrastructure.Identity;
using Appointly.Infrastructure.Persistence;
using Appointly.Infrastructure.Persistence.Repositories;
using Appointly.Infrastructure.Services;
using Appointly.Infrastructure.Services.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Appointly.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , 
            ConfigurationManager configuration
            )
        {
            services.AddAuth(configuration);
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IAdvertismentRepository, AdvertisementRepository>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AppointlyDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }


        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {

            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .ValidateOnStart();

            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer =  jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret))
                }
            );

            return services;
        }
    }
}
