using TestBackend.Internal.BusinessObjects;
using TestBackend.ServiceLibrary.Enums;

namespace TestBackend.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> GetUserNamesAsync();

        Task<ResultOperation> UpdateUserRoleAsync(UpdateUserRoleDto userDto, CancellationToken cancellationToken = default);
    }
}
