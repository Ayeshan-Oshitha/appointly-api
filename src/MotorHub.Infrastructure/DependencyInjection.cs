using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.Common.Interfaces.Services;
using MotorHub.Domain.Common.Constants;
using MotorHub.Infrastructure.Identity;
using MotorHub.Infrastructure.Persistence;
using MotorHub.Infrastructure.Persistence.Repositories;
using MotorHub.Infrastructure.Services;
using MotorHub.Infrastructure.Services.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace MotorHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services ,
            IConfiguration configuration
            )
        {
            services.AddAuth(configuration);
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IRoleChangeRepository,RoleChangeRepository>();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<MotorHubDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }


        public static IServiceCollection AddAuth(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ValidateDataAnnotations() supplies the rules; without it ValidateOnStart() has
            // nothing to run and a missing or too-short secret passes silently.
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection(JwtSettings.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            // Configured through the options pipeline rather than an eager
            // configuration.Get<JwtSettings>() so the bearer handler and JwtTokenGenerator
            // (which takes IOptions<JwtSettings>) can never read different values.
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtSettings>>((options, jwtSettingsOptions) =>
                {
                    var jwtSettings = jwtSettingsOptions.Value;

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
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AdminOnly, policy => policy.RequireRole(Roles.Admin, Roles.SuperAdmin));
                options.AddPolicy(Policies.SellerOnly, policy => policy.RequireRole(Roles.Seller));
            });

            return services;
        }
    }
}
