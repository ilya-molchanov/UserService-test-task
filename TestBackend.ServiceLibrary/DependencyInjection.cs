using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestBackend.ServiceLibrary.Repositories.Interfaces;
using TestBackend.ServiceLibrary.Repositories;

namespace TestBackend.ServiceLibrary
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStr = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped(provider =>
            {
                var connection = new SqlConnection(connectionStr);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            });

            services.AddScoped<ISqlConnectionWrapper, SqlConnectionWrapper>();
            services.AddScoped<IUsersRepository, SqlUsersRepository>();

            return services;
        }
    }
}
