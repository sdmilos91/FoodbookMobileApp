using Foodbook.MobileApp.Pages.Cook;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public HomeMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new HomeMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class HomeMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomeMasterDetailPageMenuItem> MenuItems { get; set; }

            public HomeMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<HomeMasterDetailPageMenuItem>(new[]
                {
                    new HomeMasterDetailPageMenuItem { Id = 0, Title = "Recepti", TargetType = typeof(RecipesPage) },
                    new HomeMasterDetailPageMenuItem { Id = 1, Title = "Kuvari", TargetType = typeof(CooksPage) },                    
                    new HomeMasterDetailPageMenuItem { Id = 4, Title = string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()) ? "Prijavite se" : "Odjavite se"},
                });

                if (!string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
                {
                    MenuItems.Add(new HomeMasterDetailPageMenuItem { Id = 2, Title = "Profil", TargetType = typeof(UserDetailsPage) });
                    MenuItems.Add(new HomeMasterDetailPageMenuItem { Id = 3, Title = "Podešavanja", TargetType = typeof(RecipesPage) });                    
                }

                MenuItems = new ObservableCollection<HomeMasterDetailPageMenuItem>(MenuItems.OrderBy(x => x.Id));
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}