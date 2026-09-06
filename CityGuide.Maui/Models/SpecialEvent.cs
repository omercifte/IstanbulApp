using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGuide.Maui.Models
{
    public class SpecialEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DateText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
