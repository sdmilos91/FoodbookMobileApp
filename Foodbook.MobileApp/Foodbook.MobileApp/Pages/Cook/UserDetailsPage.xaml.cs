using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserDetailsPage : ContentPage
	{
        


       
        public UserDetailsPage()
		{
			InitializeComponent ();

            ToolbarItem favourite = new ToolbarItem
            {
                Icon = "favorite",  
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItem edit = new ToolbarItem
            {
                Icon = "edit",
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItem delete = new ToolbarItem
            {
                Icon = "delete",
                Order = ToolbarItemOrder.Primary
            };

            ToolbarItems.Add(favourite);
            ToolbarItems.Add(edit);
            ToolbarItems.Add(delete);

            BindingContext = new UserDetailsViewModel();

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ImageViewPage("http://kuhinjarecepti.com/wp-content/uploads/2012/01/%C5%A0opska-salata.jpeg"));
        }
    }

    class UserDetailsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string[] Items { get; set; }

        private GridLength imageContainerHeight;

        public Command SwitchTabCommand { get;  }

        public GridLength ImageContainerHeight
        {
            get { return imageContainerHeight; }
            set { imageContainerHeight = value; OnPropertyChanged(); }
        }

        #region TAB

        private Color firstTabColor;

        public Color FirstTabColor
        {
            get { return firstTabColor; }
            set { firstTabColor = value; OnPropertyChanged(); }
        }

        private Color firstTabTextColor;

        public Color FirstTabTextColor
        {
            get { return firstTabTextColor; }
            set { firstTabTextColor = value; OnPropertyChanged(); }
        }

        private Color firstTabIndicatorColor;

        public Color FirstTabIndicatorColor
        {
            get { return firstTabIndicatorColor; }
            set { firstTabIndicatorColor = value; OnPropertyChanged(); }
        }


        private Color secondTabColor;

        public Color SecondTabColor
        {
            get { return secondTabColor; }
            set { secondTabColor = value; OnPropertyChanged(); }
        }

        private Color secondTabTextColor;

        public Color SecondTabTextColor
        {
            get { return secondTabTextColor; }
            set { secondTabTextColor = value; OnPropertyChanged(); }
        }

        private Color secondTabIndicatorColor;

        public Color SecondTabIndicatorColor
        {
            get { return secondTabIndicatorColor; }
            set { secondTabIndicatorColor = value; OnPropertyChanged(); }
        }


        private Color thirdTabColor;

        public Color ThirdTabColor
        {
            get { return thirdTabColor; }
            set { thirdTabColor = value; OnPropertyChanged(); }
        }

        private Color thirdTabTextColor;

        public Color ThirdTabTextColor
        {
            get { return thirdTabTextColor; }
            set { thirdTabTextColor = value; OnPropertyChanged(); }
        }

        private Color sthirdTabIndicatorColor;

        public Color ThirdTabIndicatorColor
        {
            get { return sthirdTabIndicatorColor; }
            set { sthirdTabIndicatorColor = value; OnPropertyChanged(); }
        }

        private bool firstContainer;

        public bool FirstContainer
        {
            get { return firstContainer; }
            set { firstContainer = value; OnPropertyChanged(); }
        }

        private bool secondContainer;

        public bool SecondContainer
        {
            get { return secondContainer; }
            set { secondContainer = value; OnPropertyChanged(); }
        }

        private bool thirdContainer;

        public bool ThirdContainer
        {
            get { return thirdContainer; }
            set { thirdContainer = value; OnPropertyChanged(); }
        }




        #endregion

        public UserDetailsViewModel()
        {
            ImageContainerHeight = new GridLength(1, GridUnitType.Star);
            SwitchTabCommand = new Command((x) => SwitchTab(x.ToString()));
            Items = new string[] { "dsa", "dsadsa", "dsad" };
            SwitchTab("1");
        }

      

        private void SwitchTab(string tab)
        {
            FirstTabColor = Color.FromHex("#EF5350");
            FirstTabIndicatorColor = Color.FromHex("#EF5350");

            SecondTabColor = Color.FromHex("#EF5350");
            SecondTabIndicatorColor = Color.FromHex("#EF5350");

            ThirdTabColor = Color.FromHex("#EF5350");
            ThirdTabIndicatorColor = Color.FromHex("#EF5350");

            FirstContainer = false;
            SecondContainer = false;
            ThirdContainer = false;


            switch (tab)
            {
                case "1":

                    FirstTabIndicatorColor = Color.FromHex("#FFD54F");
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;

                case "2":
                    SecondTabIndicatorColor = Color.FromHex("#FFD54F");
                    SecondContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;

                case "3":
                    ThirdTabIndicatorColor = Color.FromHex("#FFD54F");
                    ThirdContainer = true;
                    ImageContainerHeight = new GridLength(0, GridUnitType.Star);
                    break;
                default:
                    FirstTabIndicatorColor = Color.FromHex("#FFD54F");
                    FirstContainer = true;
                    ImageContainerHeight = new GridLength(1, GridUnitType.Star);
                    break;


            }
        }

        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}