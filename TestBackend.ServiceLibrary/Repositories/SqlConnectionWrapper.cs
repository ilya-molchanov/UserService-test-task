using Dapper;
using Microsoft.Data.SqlClient;
using TestBackend.ServiceLibrary.Repositories.Interfaces;

namespace TestBackend.ServiceLibrary.Repositories
{
    public class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private SqlConnection _connection;
        public SqlConnectionWrapper(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null) => await _connection.ExecuteAsync(sql, parameters);

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null) => await _connection.QueryAsync<T>(sql, parameters);
    }
}
