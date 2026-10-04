using Microsoft.Maui.Controls;
using ProjekatAndrejAntanaskovic.Services;
using ProjekatAndrejAntanaskovic.Views;


namespace ProjekatAndrejAntanaskovic.Views;

public partial class EntryPage : ContentPage
{
    private const string AdminPassword = AdminService.Password;

    public EntryPage()
    {
        InitializeComponent();
    }

    private void AdminPasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        AdminButton.IsEnabled =
            !string.IsNullOrWhiteSpace(AdminPasswordEntry.Text)
            && AdminPasswordEntry.Text.Length >= 4;
    }

    private async void GuestButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ServicesPage());
    }

    private async void AdminButton_Clicked(object sender, EventArgs e)
    {
        string enteredPassword = AdminPasswordEntry.Text?.Trim() ?? string.Empty;

        if (enteredPassword == AdminPassword)
        {
            await Navigation.PushAsync(new AdminPage());
        }
        else
        {
            await DisplayAlert(
                "Greška",
                "Pogrešna admin šifra.",
                "OK");

            AdminPasswordEntry.Text = string.Empty;
            AdminButton.IsEnabled = false;
        }
    }
}