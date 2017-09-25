using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Tools;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class RecipeSortViewModel : BaseViewModel
    {
        private CommonDataSortModel orderModel;

        public CommonDataSortModel OrderModel
        {
            get { return orderModel; }
            set { SetProperty(ref orderModel, value); }
        }

        private int selectedOrderIndex;

        public int SelectedOrderIndex
        {
            get { return selectedOrderIndex; }
            set { SetProperty(ref selectedOrderIndex, value); }
        }

        private int selectedOrderByIndex;

        public int SelectedOrderByIndex
        {
            get { return selectedOrderByIndex; }
            set { SetProperty(ref selectedOrderByIndex, value); }
        }


        private string orderName;

        public string OrderName
        {
            get { return orderName; }
            set { SetProperty(ref orderName, value); }
        }


        private string orderByName;

        public string OrderByName
        {
            get { return orderByName; }
            set { SetProperty(ref orderByName, value); }
        }

        public Command CancelCommand { get; }
        public Command SortCommand { get; }

        public RecipeSortViewModel(CommonDataSortModel orderModel)
        {
            OrderModel = orderModel;

            InitData();

            CancelCommand = new Command(() => Cancel());
            SortCommand = new Command(() => Sort());
        }

        private void InitData()
        {
            List<Item> orderItems = new List<Item>();
            orderItems.Add(new Item
            {
                Id = RecipeSort.ORDER_ASC,
                Name = "Rastući"
            });
            orderItems.Add(new Item
            {
                Id = RecipeSort.ORDER_DESC,
                Name = "Opadajući"
            });

            List<Item> orderByItems = new List<Item>();
            orderByItems.Add(new Item
            {
                Id = RecipeSort.NAME,
                Name = "Ime"
            });
            orderByItems.Add(new Item
            {
                Id = RecipeSort.RATING,
                Name = "Ocena"
            });
            orderByItems.Add(new Item
            {
                Id = RecipeSort.PREPARATION_TIME,
                Name = "Vreme pripreme"
            });

            OrderModel.OrderItems = orderItems;
            OrderModel.OrderByItems = orderByItems;


            SelectedOrderByIndex = OrderModel.OrderById;
            SelectedOrderIndex = OrderModel.OrderId;

            OrderName = OrderModel.OrderItems.FirstOrDefault(x => x.Id == SelectedOrderIndex).Name;
            OrderByName = OrderModel.OrderByItems.FirstOrDefault(x => x.Id == SelectedOrderByIndex).Name;

        }

        private async void Cancel()
        {
            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }

        private async void Sort()
        {
            OrderModel.OrderById = SelectedOrderByIndex != 0 ? OrderModel.OrderByItems.ElementAt(SelectedOrderByIndex).Id : RecipeSort.NAME;
            OrderModel.OrderId = SelectedOrderIndex != 0 ? OrderModel.OrderItems.ElementAt(SelectedOrderIndex).Id : RecipeSort.ORDER_ASC;

            MessagingCenter.Send(this, "SORTED", OrderModel);

            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PopPopupAsync();
        }

    }
}
