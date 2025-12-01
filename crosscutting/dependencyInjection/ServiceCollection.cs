using System.Data;
using System.Reflection;
using domain.interfaces;
using domain.interfaces.repository.read;
using infra.repository;
using infrastructure.repository.read;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace crosscutting.dependencyInjection
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.Load("application");
                
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();

            var connectionString =
                configuration["DB_CONNECTION_STRING"]
                ?? configuration.GetSection("DBConfig")["ConnectionString"];
            services.AddScoped<IDbConnection>(provider => new SqlConnection(connectionString));
            services.AddScoped(provider =>
            {
                var connection = provider.GetRequiredService<IDbConnection>();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection.BeginTransaction();
            });

            return services;
        }
    }
}