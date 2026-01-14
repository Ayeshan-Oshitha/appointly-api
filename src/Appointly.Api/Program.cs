using Appointly.Api;
using Appointly.Api.Common.Mapping;
using Appointly.Api.Middleware;
using Appointly.Application;
using Appointly.Infrastructure;
using Appointly.Infrastructure.Identity;
using Appointly.Infrastructure.Persistence;
using Appointly.Infrastructure.Persistence.Seeders;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddControllers()
        // Configure JSON options to serialize enums as strings
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    builder.Services.AddMappings();

    builder.Services.AddPresentationServices();
    builder.Services.AddApplicationService();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddDbContext<AppointlyDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddEndpointsApiExplorer();
}

var app = builder.Build();


// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();


    // Run Seeders
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppointlyDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        await LocationSeeder.SeedAsync(dbContext);
        await BrandModelSeeder.SeedAsync(dbContext);
        await AdvertismentSeeder.SeedAsync(dbContext);

        await RoleSeeder.SeedAsync(roleManager);
    }

    app.Run();
}

