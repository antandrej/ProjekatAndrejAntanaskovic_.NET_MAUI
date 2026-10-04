using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class AdminServicesPage : ContentPage
{
    public AdminServicesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RefreshData();
    }

    private async Task RefreshData()
    {
        var usluge =
            await AppServices.DataService
                .GetSveUslugeAsync();

        ServicesCollectionView.ItemsSource = usluge;
    }

    private async void Delete_Clicked(object sender,EventArgs e)
    {
        Button button = (Button)sender;

        string? id =
            button.CommandParameter?.ToString();

        if (string.IsNullOrEmpty(id))
            return;

        bool potvrda = await DisplayAlert(
            "Potvrda",
            "Da li želite da obrišete uslugu?",
            "Da",
            "Ne");

        if (!potvrda)
            return;

        await AppServices.DataService
            .ObrisiUsluguAsync(id);

        await RefreshData();
    }

    private async void Edit_Clicked(object sender,EventArgs e)
    {
        Button button = (Button)sender;

        string? id =
            button.CommandParameter?.ToString();

        if (string.IsNullOrEmpty(id))
            return;

        await Navigation.PushAsync(
            new EditServicePage(id));
    }

    private async void AddService_Clicked(object sender,EventArgs e)
    {
         await Navigation.PushAsync(
             new EditServicePage());
    }
}