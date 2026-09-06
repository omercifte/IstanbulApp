
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("Categories")]
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string CategoryName { get; set; } = string.Empty;
        [Ignore]
        public bool IsSelected { get; set; }

        [Ignore]
        public Color BackgroundColor => IsSelected ? Color.FromArgb("#0D47A1") : Color.FromArgb("#FFFFFF");

        [Ignore]
        public Color BorderColor => IsSelected ? Color.FromArgb("#0D47A1") : Color.FromArgb("#C3C6D4");

        [Ignore]
        public Color TextColor => IsSelected ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#191C1D");

    }
}
