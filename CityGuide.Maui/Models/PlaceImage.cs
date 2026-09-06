using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CityGuide.Maui.Models
{
    
        [Table("PlaceImages")]
        public class PlaceImage
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            [NotNull]
            public int PlaceId { get; set; }   // hangi mekana ait — Places tablosuna foreign key

            [NotNull]
            public string ImageUrl { get; set; } = string.Empty;

            public int OrderIndex { get; set; }  // galerideki sırası
        }

    }

