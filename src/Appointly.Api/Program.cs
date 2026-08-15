using Appointly.Api;
using Appointly.Api.Extensions;
using Appointly.Api.Middleware;
using Appointly.Application;
using Appointly.Infrastructure;
using Appointly.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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

    builder.Services.AddPresentationServices();
    builder.Services.AddApplicationService();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddDbContext<AppointlyDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    builder.Services.AddCors(options => {
        options.AddDefaultPolicy(policy =>
        {
            policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });

    builder.Services.AddEndpointsApiExplorer();
}

var app = builder.Build();

// Apply migrations and seed before the app starts serving requests.
await app.InitializeDatabaseAsync();


// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}

