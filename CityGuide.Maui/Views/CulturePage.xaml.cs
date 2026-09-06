using CityGuide.Maui.Models;
using CityGuide.Maui.Services;

namespace CityGuide.Maui.Views;

public partial class CulturePage : ContentPage
{
    private readonly AppDatabase _db = new AppDatabase();
    private List<Event> _allEvents = new List<Event>();
    private List<Category> _allCategories = new List<Category>();

    public CulturePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // --- Kategoriler ---
        var categories = await _db.GetCategoriesAsync();
        categories.Insert(0, new Category { Id = 0, CategoryName = "Tümü" });
        _allCategories = categories;
        _allCategories[0].IsSelected = true;

        CategoriesCollection.ItemsSource = _allCategories;

        // --- Etkinlikler ---
        _allEvents = await _db.GetEventsWithCategoryAsync();
        EventsCollection.ItemsSource = _allEvents;

        CategoriesCollection.SelectedItem = _allCategories[0];
    }

    private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Category category)
            return;

        // Tüm kategorilerin seçim durumunu güncelle
        foreach (var cat in _allCategories)
        {
            cat.IsSelected = (cat == category);
        }

        // Listeyi yeniden ata ki renkler tazelensin
        CategoriesCollection.ItemsSource = null;
        CategoriesCollection.ItemsSource = _allCategories;
        CategoriesCollection.SelectedItem = category;

        // Etkinlikleri filtrele
        if (category.Id == 0)
        {
            EventsCollection.ItemsSource = _allEvents;
        }
        else
        {
            EventsCollection.ItemsSource = _allEvents
                .Where(ev => ev.CategoryId == category.Id)
                .ToList();
        }
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        // Þimdilik hiçbir þey yapmýyor (senin isteðinle)
    }

    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//home");
    }

    private async void OnDiscoverTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//discover");
    }

    private async void OnEventsTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//events");
    }

    private async void OnProfileTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("profile");
    }


}