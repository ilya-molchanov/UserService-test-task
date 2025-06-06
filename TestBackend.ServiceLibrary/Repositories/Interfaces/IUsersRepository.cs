using TestBackend.ServiceLibrary.Enums;
using TestBackend.ServiceLibrary.Models;

namespace TestBackend.ServiceLibrary.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        Task<int> CreateUserAsync(SqlUser user, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<SqlUser>> GetUsersAsync();
        
        Task<ResultOperation> UpdateUserRoleAsync(int userId, int newRole, CancellationToken cancellationToken = default);
        
        Task<SqlUser> GetUserByIdAsync(int id);

        Task<bool> DoesUserExistByEmailAsync(string userEmail);
    }
}
