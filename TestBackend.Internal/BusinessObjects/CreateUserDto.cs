using TestBackend.Internal.Enums;

namespace TestBackend.Internal.BusinessObjects
{
    public class CreateUserDto
    {
        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required UserRoleCode Role { get; set; }
    }
}