using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    class RecipesPageViewModel : BaseViewModel
    {        

        private ObservableCollection<RecipeDataModel> items;

        public ObservableCollection<RecipeDataModel> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        #region TAB

        private Color firstTabColor;

        public Color FirstTabColor
        {
            get { return firstTabColor; }
            set { SetProperty(ref firstTabColor, value); }
        }

        private Color firstTabTextColor;

        public Color FirstTabTextColor
        {
            get { return firstTabTextColor; }
            set { SetProperty(ref firstTabTextColor, value); }
        }

        private Color firstTabIndicatorColor;

        public Color FirstTabIndicatorColor
        {
            get { return firstTabIndicatorColor; }
            set { SetProperty(ref firstTabIndicatorColor, value); }
        }


        private Color secondTabColor;

        public Color SecondTabColor
        {
            get { return secondTabColor; }
            set { SetProperty(ref secondTabColor, value); }
        }

        private Color secondTabTextColor;

        public Color SecondTabTextColor
        {
            get { return secondTabTextColor; }
            set { SetProperty(ref secondTabTextColor, value); }
        }

        private Color secondTabIndicatorColor;

        public Color SecondTabIndicatorColor
        {
            get { return secondTabIndicatorColor; }
            set { SetProperty(ref secondTabIndicatorColor, value); }
        }


        private Color thirdTabColor;

        public Color ThirdTabColor
        {
            get { return thirdTabColor; }
            set { SetProperty(ref thirdTabColor, value); }
        }

        private Color thirdTabTextColor;

        public Color ThirdTabTextColor
        {
            get { return thirdTabTextColor; }
            set { SetProperty(ref thirdTabTextColor, value); }
        }

        private Color thirdTabIndicatorColor;

        public Color ThirdTabIndicatorColor
        {
            get { return thirdTabIndicatorColor; }
            set { SetProperty(ref thirdTabIndicatorColor, value); }
        }

        private bool firstContainer;

        public bool FirstContainer
        {
            get { return firstContainer; }
            set { SetProperty(ref firstContainer, value); }
        }

        private bool secondContainer;

        public bool SecondContainer
        {
            get { return secondContainer; }
            set { SetProperty(ref secondContainer, value); }
        }





        #endregion

        public Command ChangeTabCommand { get; }

        public RecipesPageViewModel()
        {
            InitData();
            ChangeTab("1");
            ChangeTabCommand = new Command((x) => ChangeTab(x.ToString()));

            MessagingCenter.Subscribe<RecipeDetailViewModel, PostRecipeCommentModel>(this, "RATING_UPDATED", (sender, addedComment) =>
            {
                var temp = Items;
                var recipe = temp.Where(x => x.RecipeId == addedComment.RecipeId).FirstOrDefault();
                recipe.Comments.Add(new Comment
                {
                    CommentText = addedComment.CommentText,
                    InsertDate = addedComment.InsertDate,
                    Rating = addedComment.Rating,
                    CookName = addedComment.CookName,
                });

                recipe.Rating = recipe.Comments.Average(x => x.Rating);

                Items = new ObservableCollection<RecipeDataModel>(temp);
                
            });
        }



        private void ChangeTab(string tab)
        {
            FirstTabColor = Color.FromHex("#effcea");
            FirstTabTextColor = Color.Gray;
            FirstTabIndicatorColor = Color.FromHex("#effcea");

            SecondTabColor = Color.FromHex("#effcea");
            SecondTabTextColor = Color.Gray;
            SecondTabIndicatorColor = Color.FromHex("#effcea");

            ThirdTabColor = Color.FromHex("#effcea");
            ThirdTabTextColor = Color.Gray;
            ThirdTabIndicatorColor = Color.FromHex("#effcea");

            FirstContainer = false;
            SecondContainer = false;


            switch (tab)
            {
                case "1":

                    FirstTabTextColor = Color.Green;
                    FirstTabIndicatorColor = Color.Green;
                    FirstContainer = true;
                    break;

                case "2":
                    SecondTabTextColor = Color.Green;
                    SecondTabIndicatorColor = Color.Green;
                    SecondContainer = true;
                    break;

                case "3":
                    thirdTabTextColor = Color.Green;
                    ThirdTabIndicatorColor = Color.Green;
                    break;
                default:
                    FirstTabTextColor = Color.Green;
                    FirstTabIndicatorColor = Color.Green;
                    break;
            }
        }

        private async void InitData()
        {


            Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            var recipes = await RecipeDataService.GetRecipes();

            if (recipes != null)
                Items = new ObservableCollection<RecipeDataModel>(recipes);
            foreach (var item in Items)
            {
                item.PhotoUrl = item.Photos != null && item.Photos.Any() ? item.Photos.FirstOrDefault().Url : "http://kuhinjarecepti.com/wp-content/uploads/2012/01/%C5%A0opska-salata.jpeg";
            }

            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

        }
    }
}
