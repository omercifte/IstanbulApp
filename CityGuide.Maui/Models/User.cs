
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string FullName { get; set; } = string.Empty;

        [Unique,NotNull]
        public string Email { get; set; } = string.Empty;

        [NotNull]
        public string Password { get; set; } = string.Empty;
    }
}
