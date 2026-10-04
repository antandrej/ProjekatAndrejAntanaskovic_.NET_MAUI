using ProjekatAndrejAntanaskovic.Models;
using ProjekatAndrejAntanaskovic.Services;
using System;

namespace ProjekatAndrejAntanaskovic.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage()
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
        var sviTermini =
            await AppServices.DataService.SviTerminiAsync();

        AdminCollectionView.ItemsSource =
            sviTermini;
    }

    private async void Potvrdi_Clicked(object sender,EventArgs e)
    {
        Button button = (Button)sender;

        string terminId =
            button.CommandParameter?.ToString();

        if (string.IsNullOrEmpty(terminId))
            return;

        await AppServices.DataService
            .PromeniStatusAsync(
                terminId,
                StatusTermina.Potvrdjen);

        await RefreshData();
    }

    private async void Zavrsen_Clicked(object sender,EventArgs e)
    {
        Button button = (Button)sender;

        string terminId =
            button.CommandParameter?.ToString();

        if (string.IsNullOrEmpty(terminId))
            return;

        await AppServices.DataService
            .PromeniStatusAsync(
                terminId,
                StatusTermina.Završen);

        await RefreshData();
    }

    private async void Otkazi_Clicked(object sender,EventArgs e)
    {
        Button button = (Button)sender;

        string terminId =
            button.CommandParameter?.ToString();

        if (string.IsNullOrEmpty(terminId))
            return;

        await AppServices.DataService
            .PromeniStatusAsync(
                terminId,
                StatusTermina.Otkazan);

        await RefreshData();
    }

    private async void ManageServices_Clicked(object sender,EventArgs e)
    {
        await Navigation.PushAsync(
            new AdminServicesPage());
    }

    private async void WorkTime_Clicked(object sender,EventArgs e)
    {
        await Navigation.PushAsync(
        new WorkTimePage());
    }
}