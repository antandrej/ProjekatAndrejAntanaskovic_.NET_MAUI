using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class MyReservationsPage : ContentPage
{
    public MyReservationsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var deviceId =
            DeviceService.GetDeviceId();

        var rezervacije =
            await AppServices.DataService
                .GetIstorijuZaUredjajAsync(deviceId);

        ReservationsCollectionView.ItemsSource =
            rezervacije;
    }
}