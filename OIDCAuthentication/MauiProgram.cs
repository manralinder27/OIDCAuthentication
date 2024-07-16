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
            //    //https://ilft-ite.teamglobalexp.com
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
                Authority = "https://test-idaas.teamglobalexp.com",
                ClientId = "6d404ed6-1564-493c-a449-df816c5d36cc",   //interactive.public
                Scope = "openid",
                RedirectUri = "loadunload://",
                ClientSecret = "YTHUzdCxaq",
                ProviderInformation=new ProviderInformation { 
                    IssuerName= "https://test-teamglobalexp.verify.ibm.com/oidc/endpoint/default",
                    AuthorizeEndpoint= "https://test-idaas.teamglobalexp.com/v1.0/endpoint/default/authorize",
                    TokenEndpoint= "https://test-idaas.teamglobalexp.com/v1.0/endpoint/default/token",
                    KeySet= new IdentityModel.Jwk.JsonWebKeySet("{\"keys\": [{\r\n\"kty\": \"RSA\",\r\n\"x5t#S256\": \"j3HDN6oWpY3T1h_qezYjrFf2RG-f8sY5qSc78I8ETCM\",\r\n\"e\": \"AQAB\",\r\n\"use\": \"sig\",\r\n\"kid\": \"server\",\r\n\"x5c\": [\r\n\"MIIDQjCCAiqgAwIBAgIEWgi2MDANBgkqhkiG9w0BAQsFADBjMQkwBwYDVQQGEwAxCTAHBgNVBAgTADEJMAcGA1UEBxMAMQkwBwYDVQQKEwAxCTAHBgNVBAsTADEqMCgGA1UEAxMhdGVzdC10ZWFtZ2xvYmFsZXhwLnZlcmlmeS5pYm0uY29tMB4XDTIzMDExMDE4NTEzMloXDTMzMDEwNzE4NTEzMlowYzEJMAcGA1UEBhMAMQkwBwYDVQQIEwAxCTAHBgNVBAcTADEJMAcGA1UEChMAMQkwBwYDVQQLEwAxKjAoBgNVBAMTIXRlc3QtdGVhbWdsb2JhbGV4cC52ZXJpZnkuaWJtLmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAPZasV1eLxXSdvb3W50p9DkVWaQEuW6HM/vz2cRog+P5uHPGyLTeMaQgyXpaHrwEpvNn1WFaC7gJ8/cSHBLkXxbsYgxHT5DgPTLTvyDQqUjf33yyyz1AMOEz3Pn9yDp0RaCgcX4DiWshSSbB+ilQ0z8yUNcx4Gg57PLbLIUwn51cqrDos0gxG+8Qv0J9TURGwFKrtvNyEcX5G0SmUSMzKl6zKwxBU1KEpOaOO9R24uXhBVAPpKYRBbx8b8cHGBgtlTEty1ydeTUOwrMG3WN06IZoUNRPfp/JxhHtUeeneZBZu4AsUAFciwnIWPxSnB/PUrX0mV6UbrXL5u0wAcn0z0ECAwEAATANBgkqhkiG9w0BAQsFAAOCAQEAeGoYQVNqXXNdfw9T7VXLgCj/mtUN7ft2yJLR9gIfsUcR4YPMVy9nl4J2Gv6P0/nYRdM15+pfncG8pnwDRworogYRW5jiBZm9Sv0zz3OY59zjQehEvnWVKtwXw3HxwRrfhXMza3CBl9sk8cNT+YS7OCAxbDZ+RxTZlQGUjYJ//+TX+eevGH+2O1ptEWq0mdh5Me5wUwoA/CjPP2adO78he+6aAG6jvfmlpNC8SMuGR4VseTL9ixl1VZrIe5g9tFWmaKtoXoGDRCJdGWCsIoW9ZvMyQzR0Mywf5wiugGzPsh/w2zPVa5/aQ9pTxlv0CQAZUFkO8L2Oec4OYM/aMRUODg==\"\r\n],\r\n\"n\": \"9lqxXV4vFdJ29vdbnSn0ORVZpAS5bocz-_PZxGiD4_m4c8bItN4xpCDJeloevASm82fVYVoLuAnz9xIcEuRfFuxiDEdPkOA9MtO_INCpSN_ffLLLPUAw4TPc-f3IOnRFoKBxfgOJayFJJsH6KVDTPzJQ1zHgaDns8tsshTCfnVyqsOizSDEb7xC_Qn1NREbAUqu283IRxfkbRKZRIzMqXrMrDEFTUoSk5o471Hbi5eEFUA-kphEFvHxvxwcYGC2VMS3LXJ15NQ7CswbdY3TohmhQ1E9-n8nGEe1R56d5kFm7gCxQAVyLCchY_FKcH89StfSZXpRutcvm7TAByfTPQQ\"\r\n}\r\n]\r\n}"),
                },
                //Policy=new Policy { Discovery = new DiscoveryPolicy { RequireKeySet=false } },
                Browser = new WebAuthenticatorBrowser()
            }));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
