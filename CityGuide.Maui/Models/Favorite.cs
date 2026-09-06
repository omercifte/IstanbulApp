using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("Favorites")]
    public class Favorite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; }   // hangi kullanıcı (şimdilik sabit 1)

        [NotNull]
        public int PlaceId { get; set; }  // hangi mekan (Places tablosuna foreign key)
    }
}
