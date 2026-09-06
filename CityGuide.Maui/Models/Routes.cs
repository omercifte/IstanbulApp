using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CityGuide.Maui.Models
{
    public class Routes
    {
        [Table("Routes")]
        public class Route
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [NotNull]
            public string Title { get; set; } = string.Empty;      // "1 Günde Milano: Temel Duraklar"

            public string Description { get; set; } = string.Empty;

            public string Category { get; set; } = string.Empty;   // "Tümü", "Aile Dostu", "Lüks Alışveriş", "Sanat & Kültür"

            public string Duration { get; set; } = string.Empty;   // "8 Saat"

            public string ImageUrl { get; set; } = string.Empty;

            public string CostRange { get; set; } = string.Empty;  // "€45 - €75" (opsiyonel bilgi)
        }
    }
}
