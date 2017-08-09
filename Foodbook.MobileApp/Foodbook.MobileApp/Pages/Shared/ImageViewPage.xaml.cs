using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImageViewPage : ContentPage
	{
        public string PhotoUrl { get; set; }
        public Command PanCommand { get; }

        public ImageViewPage (string photoUrl)
		{
			InitializeComponent ();
            PhotoUrl = photoUrl;
            BindingContext = this;
		}      

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }

}