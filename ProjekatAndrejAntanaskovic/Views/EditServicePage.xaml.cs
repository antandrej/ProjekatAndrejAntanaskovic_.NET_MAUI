using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class EditServicePage : ContentPage
{
    private readonly string? _serviceId;

    public EditServicePage()
    {
        InitializeComponent();
    }

    public EditServicePage(string serviceId)
    {
        InitializeComponent();

        _serviceId = serviceId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (string.IsNullOrEmpty(_serviceId))
            return;

        PageTitleLabel.Text = "Izmena usluge";

        var usluga =
            await AppServices.DataService
                .GetUslugaByIdAsync(_serviceId);

        if (usluga == null)
            return;

        NameEntry.Text = usluga.Naziv;

        PriceEntry.Text =
            usluga.Cena.ToString();

        DurationEntry.Text =
            usluga.TrajanjeMinuti.ToString();

        DescriptionEditor.Text =
            usluga.Opis;
    }

    private async void Save_Clicked(object sender,EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert(
                "Greška",
                "Naziv je obavezan.",
                "OK");

            return;
        }

        if (!decimal.TryParse(PriceEntry.Text,out decimal cena))
        {
            await DisplayAlert(
                "Greška",
                "Cena nije ispravna.",
                "OK");

            return;
        }

        if (!int.TryParse(DurationEntry.Text,out int trajanje))
        {
            await DisplayAlert(
                "Greška",
                "Trajanje nije ispravno.",
                "OK");

            return;
        }

        if (string.IsNullOrEmpty(_serviceId))
        {
            Usluga novaUsluga = new()
            {
                Id = Guid.NewGuid().ToString(),

                Naziv = NameEntry.Text,

                Cena = cena,

                TrajanjeMinuti = trajanje,

                Opis = DescriptionEditor.Text
            };

            await AppServices.DataService
                .DodajUsluguAsync(novaUsluga);
        }
        else
        {
            Usluga izmenjena = new()
            {
                Id = _serviceId,

                Naziv = NameEntry.Text,

                Cena = cena,

                TrajanjeMinuti = trajanje,

                Opis = DescriptionEditor.Text
            };

            await AppServices.DataService
                .IzmeniUsluguAsync(izmenjena);
        }

        await DisplayAlert(
            "Uspešno",
            "Podaci su sačuvani.",
            "OK");

        await Navigation.PopAsync();
    }
}