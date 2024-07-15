using OIDCAuthentication.ViewModels;

namespace OIDCAuthentication
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel mainViewModel)
        {
            Shell.SetNavBarIsVisible(this, false);
            InitializeComponent();
            BindingContext= mainViewModel;
       }
        
    }

}
