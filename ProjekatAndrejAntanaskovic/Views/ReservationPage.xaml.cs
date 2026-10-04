using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class ReservationPage : ContentPage
{
    private readonly Usluga _usluga;
    private readonly DateTime _datumVreme;

    private readonly MockDataService _dataService;

    public ReservationPage(
        Usluga usluga,
        DateTime datumVreme)
    {
        InitializeComponent();

        _usluga = usluga;
        _datumVreme = datumVreme;

        _dataService = AppServices.DataService;

        ServiceLabel.Text =
            $"{usluga.Naziv} - {usluga.Cena} din";

        DateTimeLabel.Text =
            datumVreme.ToString("dd.MM.yyyy HH:mm");
    }

    private async void ConfirmReservation_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert(
                "Greška",
                "Unesite ime i prezime.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            await DisplayAlert(
                "Greška",
                "Unesite telefon.",
                "OK");

            return;
        }

        Termin termin = new Termin
        {
            UslugaId = _usluga.Id,
            NazivUsluge = _usluga.Naziv,
            ImeKlijenta = NameEntry.Text,
            TelefonKlijenta = PhoneEntry.Text,
            UredjajId = DeviceService.GetDeviceId(),
            DatumVreme = _datumVreme,
            TrajanjeMinuti = _usluga.TrajanjeMinuti,
            Status = StatusTermina.Potvrdjen
        };

        bool success = await _dataService.ZakaziTerminAsync(termin);

        if (!success)
        {
            await DisplayAlert(
                "Greška",
                "Termin je već zauzet.",
                "OK");

            return;
        }

        await DisplayAlert(
            "Uspešno",
            "Termin je rezervisan.",
            "OK");

        await Navigation.PopAsync();
    }
}