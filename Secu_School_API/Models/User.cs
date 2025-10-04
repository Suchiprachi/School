namespace Secu_School_API.Models
{
    public class User
    {
        public string UserName { get; set; } = string.Empty;

        public string UserType { get; set; } = string.Empty;

        public string EntityName { get; set; } = string.Empty;

        public string Photos { get; set; } = string.Empty;

        public int SchoolId { get; set; }
    }
}
