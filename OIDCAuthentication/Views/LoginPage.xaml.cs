using OIDCAuthentication.ViewModels.Auth;

namespace OIDCAuthentication.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginViewModel)
	{
		InitializeComponent();
		BindingContext=loginViewModel;
		
	}
}