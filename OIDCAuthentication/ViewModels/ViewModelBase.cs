using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OIDCAuthentication.ViewModels
{
   public partial class ViewModelBase: ObservableObject
    {
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        [ObservableProperty]
        bool isBusy;
        public bool IsNotBusy=>!isBusy;
    }
}
