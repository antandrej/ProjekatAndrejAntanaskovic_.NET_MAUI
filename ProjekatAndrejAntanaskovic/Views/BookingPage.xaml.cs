using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class BookingPage : ContentPage
{
    private readonly MockDataService _dataService;

    private readonly Usluga _selectedService;

    public BookingPage(Usluga selectedService)
    {
        InitializeComponent();

        _dataService = AppServices.DataService;

        _selectedService = selectedService;

        ServiceNameLabel.Text =
            $"{selectedService.Naziv} ({selectedService.Cena} din)";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadAvailableSlots();
    }

    private async Task LoadAvailableSlots()
    {
        var slots = await _dataService.GetSlobodniSlotoviAsync(
            BookingDatePicker.Date,
            _selectedService.TrajanjeMinuti);

        SlotsCollectionView.ItemsSource =
            slots.Select(x => x.ToString("HH:mm")).ToList();
    }

    private async void BookingDatePicker_DateSelected(
        object sender,
        DateChangedEventArgs e)
    {
        await LoadAvailableSlots();
    }

    private async void SlotButton_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        string selectedTime =
        button.CommandParameter.ToString();

        DateTime selectedDateTime =
        DateTime.Parse(
        $"{BookingDatePicker.Date:yyyy-MM-dd} {selectedTime}");

        await Navigation.PushAsync(
        new ReservationPage(
        _selectedService,
        selectedDateTime));
    }
}