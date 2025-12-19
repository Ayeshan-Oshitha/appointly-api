using Appointly.Api.Common.Mapping;
using Appointly.Api.Middleware;
using Appointly.Application;
using Appointly.Infrastructure;
using Appointly.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    builder.Services.AddControllers();

    builder.Services.AddMappings();

    builder.Services.AddApplicationService();
    builder.Services.AddInfrastructureServices();

    builder.Services.AddDbContext<AppointlyDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

    builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Appointly",
            ValidAudience = "AppointlyUsers",
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("jshhdgshskdndHHYYnsddfdjf76474734854546kfg"))
        }
        );

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
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
    app.Run();
}

