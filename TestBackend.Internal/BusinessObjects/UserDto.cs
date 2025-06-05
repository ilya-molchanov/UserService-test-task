using TestBackend.Internal.Enums;

namespace TestBackend.Internal.BusinessObjects
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public UserRoleCode Role { get; set; }
    }
}