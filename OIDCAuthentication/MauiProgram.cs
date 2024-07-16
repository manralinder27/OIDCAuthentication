using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OIDCAuthentication.ViewModels;
using OIDCAuthentication.ViewModels.Auth;


namespace OIDCAuthentication
{
    public static class MauiProgram
    {       
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<Views.LoginPage>();
            builder.Services.AddSingleton<MainPage>();

            
            builder.Services.AddTransient<WebAuthenticatorBrowser>();


            // setup OidcClient
            //builder.Services.AddTransient<OidcClient>(sp => new OidcClient(new OidcClientOptions
            //{
            //    
            //    // https://demo.duendesoftware.com

            //    Authority = "https://demo.duendesoftware.com",
            //    ClientId = "interactive.public",   //interactive.public
            //    Scope = "openid profile api",
            //    RedirectUri = "loadunload://",
            //    //ClientSecret = "YTHUzdCxaq",
            //    Browser = new WebAuthenticatorBrowser()

            //}));

            builder.Services.AddSingleton(new OidcClient(new()
            {
                Authority = "",
                ClientId = "",   //interactive.public
                Scope = "openid",
                RedirectUri = "OIDCAuthentication://callback",
                ClientSecret = "",
                ProviderInformation=new ProviderInformation { 
                    IssuerName= "",
                    AuthorizeEndpoint= "",
                    TokenEndpoint= "",
                    KeySet= new IdentityModel.Jwk.JsonWebKeySet(""),
                },                
                Browser = new WebAuthenticatorBrowser()
            }));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
