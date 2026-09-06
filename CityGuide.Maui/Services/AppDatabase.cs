using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityGuide.Maui.Models;
using SQLite;
using static CityGuide.Maui.Models.Routes;
using static CityGuide.Maui.Models.RouteStops;

namespace CityGuide.Maui.Services
{
    public class AppDatabase
    {
        private SQLiteAsyncConnection _database;

        // Veritabanını hazırlar: bağlantıyı açar ve tabloları oluşturur.
        // Bir kez kurulduktan sonra tekrar kurmaz.
        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "cityguide.db");

            // Dosyanın tam yolunu görelim (SQLite Browser'da açmak için)
            System.Diagnostics.Debug.WriteLine($"[DB PATH] {dbPath}");

            _database = new SQLiteAsyncConnection(dbPath);

            // Code-first: modellere bakıp tabloları oluştur (yoksa)
            await _database.CreateTableAsync<Category>();
            await _database.CreateTableAsync<Event>();
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Favorite>();
            await _database.CreateTableAsync<Place>();
            await _database.CreateTableAsync<TransportLine>();
            await _database.CreateTableAsync<Route>();
            await _database.CreateTableAsync<RouteStop>();
            await _database.CreateTableAsync<FoodPlace>();
            await _database.CreateTableAsync<PlaceImage>();
        }

        // --- Okuma metotları ---

        public async Task<List<Category>> GetCategoriesAsync()
        {
            await InitAsync();
            return await _database.Table<Category>().ToListAsync();
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            await InitAsync();
            return await _database.Table<Event>().ToListAsync();
        }

        // Etkinlikleri çeker VE her birinin kategori adını doldurur (foreign key eşleştirme)
        public async Task<List<Event>> GetEventsWithCategoryAsync()
        {
            await InitAsync();

            // İki tabloyu da çek
            var events = await _database.Table<Event>().ToListAsync();
            var categories = await _database.Table<Category>().ToListAsync();

            // Her etkinlik için, CategoryId'sine uyan kategoriyi bul ve adını yaz
            foreach (var ev in events)
            {
                var matchingCategory = categories.FirstOrDefault(c => c.Id == ev.CategoryId);
                if (matchingCategory is not null)
                {
                    ev.CategoryName = matchingCategory.CategoryName;
                }
                else
                {
                    ev.CategoryName = "Bilinmeyen";
                }
            }

            return events;
        }


        // --- Yazma metotları (uygulama içinden eklemek istersen) ---

        public async Task<int> AddCategoryAsync(Category category)
        {
            await InitAsync();
            return await _database.InsertAsync(category);
        }

        public async Task<int> AddEventAsync(Event newEvent)
        {
            await InitAsync();
            return await _database.InsertAsync(newEvent);
        }

        // --- Kullanıcı metotları ---

        // Yeni kullanıcı ekler. E-posta zaten varsa [Unique] yüzünden hata fırlatır.
        public async Task<int> AddUserAsync(User user)
        {
            await InitAsync();
            return await _database.InsertAsync(user);
        }

        // E-postaya göre kullanıcı arar. Bulamazsa null döner.
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await InitAsync();
            return await _database.Table<User>()
                                  .Where(u => u.Email == email)
                                  .FirstOrDefaultAsync();
        }

        // --- Mekan (Place) metotları ---

        public async Task<List<Place>> GetPlacesAsync()
        {
            await InitAsync();
            return await _database.Table<Place>().ToListAsync();
        }

        // --- Favori metotları ---

        // Bir mekanı favorilere ekler (insert)
        public async Task<int> AddFavoriteAsync(int userId, int placeId)
        {
            await InitAsync();
            var favorite = new Favorite { UserId = userId, PlaceId = placeId };
            return await _database.InsertAsync(favorite);
        }

        // Bir mekanı favorilerden çıkarır (delete)
        public async Task<int> RemoveFavoriteAsync(int userId, int placeId)
        {
            await InitAsync();
            var existing = await _database.Table<Favorite>()
                .Where(f => f.UserId == userId && f.PlaceId == placeId)
                .FirstOrDefaultAsync();

            if (existing is null)
                return 0;

            return await _database.DeleteAsync(existing);
        }

        // Bir mekan favori mi? (true/false)
        public async Task<bool> IsFavoriteAsync(int userId, int placeId)
        {
            await InitAsync();
            var existing = await _database.Table<Favorite>()
                .Where(f => f.UserId == userId && f.PlaceId == placeId)
                .FirstOrDefaultAsync();

            return existing is not null;
        }

        // Bir kullanıcının favorilediği mekanları getirir (join mantığı)
        public async Task<List<Place>> GetFavoritePlacesAsync(int userId)
        {
            await InitAsync();

            // 1) Bu kullanıcının favori kayıtlarını çek
            var favorites = await _database.Table<Favorite>()
                .Where(f => f.UserId == userId)
                .ToListAsync();

            // 2) Favorilenen PlaceId'leri topla
            var favoritePlaceIds = favorites.Select(f => f.PlaceId).ToList();

            // 3) Bu Id'lere sahip mekanları çek
            var allPlaces = await _database.Table<Place>().ToListAsync();
            var favoritePlaces = allPlaces
                .Where(p => favoritePlaceIds.Contains(p.Id))
                .ToList();

            return favoritePlaces;
        }

        //ulaşım hatları

        public async Task<List<TransportLine>> GetTransportLinesAsync()
        {
            await InitAsync();
            return await _database.Table<TransportLine>().ToListAsync();
        }

        //türe göre filtrelenmiş hal

        public async Task<List<TransportLine>> GetTransportLinesAsync(string type)
        {
            await InitAsync();
            return await _database.Table<TransportLine>()
                .Where(t=>t.Type == type)
                .ToListAsync();
        }


        // Tüm rotaları getirir (durakları olmadan)
        public async Task<List<Route>> GetRoutesAsync()
        {
            await InitAsync();
            return await _database.Table<Route>().ToListAsync();
        }

        // Belirli bir rotanın duraklarını, sıralı şekilde getirir
        public async Task<List<RouteStop>> GetRouteStopsAsync(int routeId)
        {
            await InitAsync();
            return await _database.Table<RouteStop>()
                                  .Where(s => s.RouteId == routeId)
                                  .OrderBy(s => s.OrderIndex)
                                  .ToListAsync();
        }


        public async Task<List<FoodPlace>> GetFoodPlacesAsync()
        {
            await InitAsync();
            return await _database.Table<FoodPlace>().ToListAsync();
        }

        public async Task<List<PlaceImage>> GetPlaceImagesAsync(int placeId)
        {
            await InitAsync();
            return await _database.Table<PlaceImage>()
                .Where(img=> img.PlaceId==placeId)
                .OrderBy(img=> img.OrderIndex)
                .ToListAsync();
        }

    }
}