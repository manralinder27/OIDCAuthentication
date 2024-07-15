using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OIDCAuthentication.ViewModels
{
    [QueryProperty("Token", "Token")]
    public partial class MainViewModel:ViewModelBase
    {
        protected readonly OidcClient _oidcClient;
        protected readonly HttpClient _httpClient;
        IConnectivity _connectivity;

        public MainViewModel(OidcClient oidcClient, IConnectivity connectivity)
        {
            _oidcClient = oidcClient;
           // _httpClient = httpClient;
            _connectivity = connectivity;
        }
        [ObservableProperty] private string token;

        [RelayCommand]
        async Task LogoutAsync()
        {
            try
            {
                var loginResult = await _oidcClient.LogoutAsync(new LogoutRequest());
                await Shell.Current.DisplayAlert("Result", "Success", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.ToString(), "ok");
            }
        }

        [RelayCommand]
        async Task CallApiAsync()
        {
            if (IsBusy)
                return;
            try
            {
                if (_connectivity.NetworkAccess is not NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Offline", "Check the internet connection!", "Ok");
                    return;
                }

                IsBusy = true;
                var client = new HttpClient();
                client.SetBearerToken(token);


                //_httpClient.SetBearerToken(token);
                var response = await client.GetAsync("https://demo.duendesoftware.com/api/test\"");
                if (!response.IsSuccessStatusCode)
                    await Shell.Current.DisplayAlert("Api Error", $"{response.StatusCode}", "ok");


                var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
                var formatted = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
                await Shell.Current.DisplayAlert("Token Claims", formatted, "ok");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.ToString(), "ok");
            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}
