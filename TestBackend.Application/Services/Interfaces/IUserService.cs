using TestBackend.Internal.BusinessObjects;

namespace TestBackend.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default);

        Task UpdateUserRoleAsync(UpdateUserRoleDto userDto, CancellationToken cancellationToken = default);
    }
}
