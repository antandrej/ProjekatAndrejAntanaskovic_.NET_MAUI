using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;
using ProjekatAndrejAntanaskovic.Views;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class ServicesPage : ContentPage
{
    private readonly MockDataService _dataService;

    public ServicesPage()
    {
        InitializeComponent();

        _dataService = AppServices.DataService;
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

        var selectedService =
        button.CommandParameter as Usluga;

        if (selectedService != null)
        {
            await Navigation.PushAsync(
                new BookingPage(selectedService));
        }
    }

    private async void MyReservationsButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyReservationsPage());
    }
}