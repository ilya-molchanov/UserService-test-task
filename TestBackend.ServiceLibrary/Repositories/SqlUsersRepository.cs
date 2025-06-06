using Dapper;
using TestBackend.ServiceLibrary.Enums;
using TestBackend.ServiceLibrary.Models;
using TestBackend.ServiceLibrary.Repositories.Interfaces;

namespace TestBackend.ServiceLibrary.Repositories
{
    public class SqlUsersRepository : IUsersRepository
    {
        private readonly ISqlConnectionWrapper _connectionWrapper;

        public SqlUsersRepository(ISqlConnectionWrapper connectionWrapper)
        {
            _connectionWrapper = connectionWrapper;
        }

        public async Task<int> CreateUserAsync(SqlUser user, CancellationToken cancellationToken = default)
        {
            var query = "INSERT INTO [dbo].[Users] ([Name], [Email], [Password], [Role]) VALUES (@name, @email, @password, @role); SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new DynamicParameters();
            parameters.Add("@name", user.Name);
            parameters.Add("@email", user.Email);
            parameters.Add("@password", user.Password);
            parameters.Add("@role", user.Role);

            return await _connectionWrapper.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<SqlUser>> GetUsersAsync()
        {
            var query = "SELECT [Id], [Name], [Email], [Password], [Role] FROM [dbo].[Users]";

            return await _connectionWrapper.QueryAsync<SqlUser>(query);
        }

        public async Task<SqlUser?> GetUserByIdAsync(int id)
        {
            var query = "SELECT [Id], [Name], [Email], [Password], [Role] FROM [dbo].[Users] WHERE [Id] = @userId";

            var parameters = new DynamicParameters();
            parameters.Add("@userId", id);

            return (await _connectionWrapper.QueryAsync<SqlUser>(query, parameters)).FirstOrDefault();
        }

        public async Task<ResultOperation> UpdateUserRoleAsync(int userId, int newRole, CancellationToken cancellationToken = default)
        {
            var query = "UPDATE [dbo].[Users] SET [Role] = @role WHERE [Id] = @userId";

            var parameters = new DynamicParameters();
            parameters.Add("@userId", userId);
            parameters.Add("@role", newRole);

            var result = await _connectionWrapper.ExecuteAsync(query, parameters);
            ResultOperation resultEnum = (ResultOperation)(result);

            return resultEnum;
        }

        public async Task<bool> DoesUserExistByEmailAsync(string userEmail)
        {
            var query = "SELECT TOP (1) [Id] FROM [dbo].[Users] WHERE [Email] = '@userEmail'";

            var parameters = new DynamicParameters();
            parameters.Add("@userEmail", userEmail);

            return (await _connectionWrapper.QueryAsync<SqlUser>(query, parameters)) != null;
        }
    }
}
