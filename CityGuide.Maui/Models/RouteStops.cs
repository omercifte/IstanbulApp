using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CityGuide.Maui.Models
{
    public class RouteStops
    {
        [Table("RouteStops")]
        public class RouteStop
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [NotNull]
            public int RouteId { get; set; }   // hangi rotaya ait — Routes tablosuna foreign key

            [NotNull]
            public string Name { get; set; } = string.Empty;        // "Duomo di Milano"

            public string Description { get; set; } = string.Empty; // "Katedral çatısında gün doğumuyla başlayın."

            public string IconGlyph { get; set; } = string.Empty;   // Material Symbols kodu, örn. "\ue898" (church)

            public int OrderIndex { get; set; }  // durağın sırası (1, 2, 3...)
        }
    }
}
