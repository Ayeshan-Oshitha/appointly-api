using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace MotorHub.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddSwaggerUI();

            // [ApiController] model-validation failures default to RFC7807 ProblemDetails, which does
            // not match the {title, error, errorCode} envelope ErrorHandlingMiddleware returns for
            // everything else. Reshape it so clients only ever parse one error contract.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage)
                        .Where(message => !string.IsNullOrWhiteSpace(message));

                    return new BadRequestObjectResult(new
                    {
                        title = "Bad Request",
                        error = string.Join("; ", errors),
                        errorCode = StatusCodes.Status400BadRequest
                    });
                };
            });

            return services;
        }


        public static IServiceCollection AddSwaggerUI(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Advertisement Website",
                    Version = "v1"
                });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Description = "Enter JWT token like: {your-token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                    Array.Empty<string>()
                    }
                });

            });
            return services;
        }
    }
}
