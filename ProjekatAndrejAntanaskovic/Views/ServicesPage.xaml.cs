using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class ServicesPage : ContentPage
{
    private readonly MockDataService _dataService;

    public ServicesPage()
    {
        InitializeComponent();

        _dataService = new MockDataService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var usluge = await _dataService.GetUslugeAsync();

        ServicesCollectionView.ItemsSource = usluge;
    }

    private async void SelectServiceButton_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        string serviceId = button.CommandParameter.ToString();

        await DisplayAlert(
            "Izabrana usluga",
            $"ID usluge: {serviceId}",
            "OK");

        // kasnije:
        // await Navigation.PushAsync(new BookingPage(serviceId));
    }
}