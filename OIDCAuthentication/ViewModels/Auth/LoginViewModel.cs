using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OIDCAuthentication.ViewModels.Auth
{
    public partial class LoginViewModel : ViewModelBase
    {
        protected readonly OidcClient _client;
        IConnectivity _connectivity;
        public LoginViewModel(OidcClient client, IConnectivity connectivity)
        {
            _client = client;
            _connectivity = connectivity;
        }


        [RelayCommand]
        async Task LoginAsync()
        {
           
            if (IsBusy)
                return;

            try
            {
                if (_connectivity.NetworkAccess is not NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Offline", "Check internet connection!", "Ok");
                    return;
                }
                IsBusy = true;

                var client = new HttpClient();
            //    var discoResponse = await client.GetDiscoveryDocumentAsync(
            //        new DiscoveryDocumentRequest
            //        {
            //            Address = "https://test-idaas.teamglobalexp.com/oidc/endpoint/default/.well-known/openid-configuration",
            //            Policy =
            //            {
            //ValidateIssuerName = false,
            //ValidateEndpoints = false,
            //            },
            //        }
            //    );

                var loginResult = await _client.LoginAsync(new LoginRequest());
                if (loginResult.IsError)
                    return;

                await Shell.Current.DisplayAlert("Login Result", "Access Token is:\n\n" + loginResult.AccessToken, "Close");
                await Shell.Current.GoToAsync($"{nameof(MainPage)}", true,
                    new Dictionary<string, object>
                    {
                    {"Token", loginResult.AccessToken }
                    });
                IsBusy = false;

                // Application.Current.MainPage = new MainPage();
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
