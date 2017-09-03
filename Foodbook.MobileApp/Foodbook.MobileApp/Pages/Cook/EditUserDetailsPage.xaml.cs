using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages.Cook
{
    public partial class EditUserDetailsPage : ContentPage
    {
        public EditUserDetailsPage(ResponseCookModel cook)
        {
            InitializeComponent();
            BindingContext = new EditUserDetailsPageViewModel(cook);
        }
    }
}