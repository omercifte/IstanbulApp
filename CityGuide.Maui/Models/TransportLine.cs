
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("TransportLines")]
    public class TransportLine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Type { get; set; } = string.Empty;

        [NotNull]
        public string LineCode { get; set; } = string.Empty;

        [NotNull]
        public string LineName { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
    }
}
