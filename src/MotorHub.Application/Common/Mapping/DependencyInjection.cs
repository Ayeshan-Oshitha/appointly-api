using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MotorHub.Application.Common.Mapping
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(DependencyInjection).Assembly);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
    }
}
