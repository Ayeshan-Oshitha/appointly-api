using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.Common.Interfaces.Services;
using Appointly.Infrastructure.Identity;
using Appointly.Infrastructure.Persistence;
using Appointly.Infrastructure.Persistence.Repositories;
using Appointly.Infrastructure.Services;
using Appointly.Infrastructure.Services.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

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
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IRoleChangeRepository,RoleChangeRepository>();

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

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options => 
                
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                        RoleClaimType = ClaimTypes.Role
                    };



                    // Custom Error Response for 401 and 403
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            string errorMessage = "Authentication token is missing or invalid.";

                            if (context.ErrorDescription != null)
                            {
                                if (context.ErrorDescription.Contains("expired"))
                                    errorMessage = "Your authentication token has expired. Please log in again.";
                                else if (context.ErrorDescription.Contains("signature"))
                                    errorMessage = "Invalid authentication token.";
                            }
                            else if (!context.Request.Headers.ContainsKey("Authorization"))
                            {
                                errorMessage = "Authentication token is required. Please provide a valid token.";
                            }

                            var response = new
                            {
                                title = "Unauthorized",
                                error = errorMessage,
                                errorCode = 401
                            };

                            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        },

                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            var response = new
                            {
                                title = "Forbidden",
                                error = "You do not have permission to access this resource.",
                                errorCode = 403
                            };

                            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        }
                    };

                }




            );

            services.AddAuthorization();

            return services;
        }
    }
}
