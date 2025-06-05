using AutoMapper;
using BackendTest.WebApi.Filters.Exception;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Description;
using TestBackend.Application.Services.Interfaces;
using TestBackend.Internal.BusinessObjects;

namespace BackendTestWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(CustomExceptionFilter))]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;

        public UserController(ILogger<UserController> logger, IUserService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Get UserNames
        /// </summary>
        /// <returns>
        /// list of names
        /// </returns>
        [HttpGet(Name = "GetUserNames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseType(typeof(IEnumerable<string>))]
        public async Task<IActionResult> GetUserNames()
        {
            _logger.LogDebug("UserController.GetUserNames was called...");

            var users = new List<UserDto>();
            users = (await _service.GetUsersAsync()).ToList();

            var userNames = users.Select(u => u.Name);

            return Ok(userNames);
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="newUser">New user entity</param>
        /// <returns>Id of the created user</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseType(typeof(ActionResult<int>))]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDto newUser)
        {
            _logger.LogDebug($"Insert operation has been started");

            var userId = await _service.CreateUserAsync(newUser);            

            _logger.LogDebug($"New user entity has been inserted with an id '{userId}'");

            return Created(string.Empty, userId);
        }

        /// <summary>
        /// Edit a given user role
        /// </summary>
        /// <param name="user">A given user entity</param>
        ///  Returns NoContent result.
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ResponseType(typeof(ActionResult))]
        public async Task<ActionResult> UpdateUserRole([FromForm] UpdateUserRoleDto user)
        {
            _logger.LogDebug($"UpdateUserRole operation has been started");

            await _service.UpdateUserRoleAsync(user);

            _logger.LogDebug($"A given user role for user with an id '{user.Id}' has been updated");

            return NoContent();
        }
    }
}
