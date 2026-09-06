using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityGuide.Maui.Services
{
    public static class CurrentSession
    {
        public static int UserId { get; set; }
        public static string FullName { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;

        public static void Clear()
        {
            UserId = 0;
            FullName = string.Empty;
            Email = string.Empty;
        }
    }
}
