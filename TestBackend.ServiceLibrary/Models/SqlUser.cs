namespace TestBackend.ServiceLibrary.Models
{
    public class SqlUser
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int Role { get; set; }
    }
}
