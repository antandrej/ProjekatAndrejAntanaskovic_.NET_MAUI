using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class WorkTimePage : ContentPage
{
    public WorkTimePage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var radnoVreme =
            await AppServices.DataService
                .GetRadnoVremeAsync();

        StartTimePicker.Time =
            radnoVreme.PocetakRadnogVremena;

        EndTimePicker.Time =
            radnoVreme.KrajRadnogVremena;

        IntervalEntry.Text =
            radnoVreme.IntervalMinuti.ToString();
    }

    private async void Save_Clicked(object sender,EventArgs e)
    {
        if (!int.TryParse(
            IntervalEntry.Text,
            out int interval))
        {
            await DisplayAlert(
                "Greška",
                "Interval nije validan.",
                "OK");

            return;
        }

        RadnoVreme novoRadnoVreme =
            new()
            {
                PocetakRadnogVremena =
                    StartTimePicker.Time,

                KrajRadnogVremena =
                    EndTimePicker.Time,

                IntervalMinuti =
                    interval
            };

        await AppServices.DataService.SacuvajRadnoVremeAsync(novoRadnoVreme);

        await DisplayAlert(
            "Uspešno",
            "Radno vreme je sačuvano.",
            "OK");

        await Navigation.PopAsync();
    }
}