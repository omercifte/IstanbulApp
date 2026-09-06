
using SQLite;

namespace CityGuide.Maui.Models
{
    [Table("FoodPlaces")]
    public class FoodPlace
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        [NotNull]
        public string CuisineType { get; set; } = string.Empty;   // "Pasta", "Pizza", "Fine Dining", "Aperitivo", "Gelato"

        public string PriceRange { get; set; } = string.Empty;    // "€€€€"

        public double Rating { get; set; }

        public string ImageUrl { get; set; } = string.Empty;
    }


}
