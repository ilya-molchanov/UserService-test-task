using AutoMapper;
using BackendTest.Internal.Exceptions.Models;
using BackendTest.Internal.Exceptions;
using Microsoft.Extensions.Logging;
using TestBackend.Application.Services.Interfaces;
using TestBackend.Internal.BusinessObjects;
using TestBackend.ServiceLibrary.Enums;
using TestBackend.ServiceLibrary.Models;
using TestBackend.ServiceLibrary.Repositories.Interfaces;

namespace TestBackend.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUsersRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _usersRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> CreateUserAsync(CreateUserDto userDto, CancellationToken cancellationToken = default)
        {
            if (userDto == null)
            {
                _logger.LogError("The user model is null.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The user model is null."));
            }

            return await _usersRepository.CreateUserAsync(_mapper.Map<SqlUser>(userDto), cancellationToken);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken = default)
        {
            return (await _usersRepository.GetUsersAsync(cancellationToken)).Select(_mapper.Map<UserDto>).ToList();
        }

        public async Task UpdateUserRoleAsync(UpdateUserRoleDto userDto, CancellationToken cancellationToken = default)
        {
            if (userDto == null)
            {
                _logger.LogError("The user model is null.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The user model is null."));
            }

            var userWithGivenId = await _usersRepository.GetUserByIdAsync(userDto.Id, cancellationToken);

            if (userWithGivenId == null)
            {
                _logger.LogError($"The user with a given id {userDto.Id} does not exist or has been deleted.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.ItemNotFound,
                    new InternalBusinessData($"The user with a given id {userDto.Id} does not exist or has been deleted."));
            }

            var result = await _usersRepository.UpdateUserRoleAsync(userDto.Id, (int)userDto.Role, cancellationToken);

            if (result == ResultOperation.Error)
            {
                _logger.LogError($"The user with a given id {userDto.Id} can't be updated");
                throw new InternalApiBusinessException(InternalApiErrorCodes.CannotUpdate,
                    new InternalBusinessData($"The user with a given id {userDto.Id} can't be updated"));
            }
        }
    }
}
