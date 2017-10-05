using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            NotificationSwitch.IsToggled = Helpers.Settings.PushNotificationsSettings;
            NotificationSwitch.Toggled += NotificationSwitch_Toggled;

        }

        private void NotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Helpers.Settings.PushNotificationsSettings = e.Value;
        }
    }
}