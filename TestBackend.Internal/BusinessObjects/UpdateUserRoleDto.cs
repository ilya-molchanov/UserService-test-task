using TestBackend.Internal.Enums;

namespace TestBackend.Internal.BusinessObjects
{
    public class UpdateUserRoleDto
    {
        public int Id { get; set; }

        public UserRoleCode Role { get; set; }
    }
}