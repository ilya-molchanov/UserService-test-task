using AutoMapper;
using BackendTest.Internal.Exceptions.Models;
using BackendTest.Internal.Exceptions;
using Microsoft.Extensions.Logging;
using TestBackend.Application.Services.Interfaces;
using TestBackend.Internal.BusinessObjects;
using TestBackend.ServiceLibrary.Enums;
using TestBackend.ServiceLibrary.Models;
using TestBackend.ServiceLibrary.Repositories.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace BackendTestWebAPI.Application.Services
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

            userDto.Name.Trim();
            userDto.Email.Trim();
            userDto.Password.Trim();

            if (string.IsNullOrEmpty(userDto.Name))
            {
                _logger.LogError("The user name is empty.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.EmptyName,
                    new InternalBusinessData("The user name is empty."));
            }
            else if (string.IsNullOrEmpty(userDto.Email))
            {
                _logger.LogError("The user email is empty.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.EmptyEmail,
                    new InternalBusinessData("The user email is empty."));
            }
            else if (string.IsNullOrEmpty(userDto.Password))
            {
                _logger.LogError("The user password is empty.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.EmptyPassword,
                    new InternalBusinessData("The user password is empty."));
            }

            if (!Regex.IsMatch(userDto.Email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase))
            {
                _logger.LogError($"This email {userDto.Email} is invalid.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.InvalidEmail,
                    new InternalBusinessData($"This email {userDto.Email} is invalid."));
            }

            var user = _mapper.Map<SqlUser>(userDto);

            var userExists = await _usersRepository.DoesUserExistByEmailAsync(user.Email);

            if (userExists)
            {
                _logger.LogError($"The user with this email {userDto.Email} already exists.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.UserWithGivenEmailAlreadyExists,
                    new InternalBusinessData($"The user with this email {userDto.Email} already exists."));
            }

            return await _usersRepository.CreateUserAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetUserNamesAsync()
        {
            return (await _usersRepository.GetUsersAsync()).Select(n => n.Name).ToList();
        }

        public async Task<ResultOperation> UpdateUserRoleAsync(UpdateUserRoleDto userDto, CancellationToken cancellationToken = default)
        {
            if (userDto == null)
            {
                _logger.LogError("The user model is null.");
                throw new InternalApiBusinessException(InternalApiErrorCodes.BadRequest,
                    new InternalBusinessData("The user model is null."));
            }

            var userWithGivenId = await _usersRepository.GetUserByIdAsync(userDto.Id);

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

            return result;
        }
    }
}
