using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("Events")]
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Title { get; set; } = string.Empty;

        [NotNull]
        public int CategoryId { get; set; }
        public string DateText { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public double Rating { get; set; }

        [Ignore]
        public string CategoryName { get; set; } = string.Empty;

        [Ignore]
        public Color BadgeColor => CategoryName switch
        {
            "Konserler" => Color.FromArgb("#E1BEE7"),
            "Tiyatro" => Color.FromArgb("#FFDBCD"),
            "Sergi" => Color.FromArgb("#D9E2FF"),
            "Futbol Maçı" => Color.FromArgb("#C8E6C9"),
            _ => Color.FromArgb("#EDEEEF")
        };

        [Ignore]
        public Color BadgeTextColor => CategoryName switch
        {
            "Konserler" => Color.FromArgb("#4A148C"),
            "Tiyatro" => Color.FromArgb("#7D2D00"),
            "Sergi" => Color.FromArgb("#0D47A1"),
            "Futbol Maçı" => Color.FromArgb("#1B5E20"),
            _ => Color.FromArgb("#455A64")
        };


    }
}
