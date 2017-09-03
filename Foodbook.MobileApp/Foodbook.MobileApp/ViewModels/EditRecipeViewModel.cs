using Acr.UserDialogs;
using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
using Foodbook.MobileApp.Pages;
using Foodbook.MobileApp.Pages.Recipe;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class EditRecipeViewModel : BaseViewModel
    {
        public PostRecipeModel DataModel { get; set; }
        private List<PhotoModel> mDeletedPhotos;
        private RecipeDataModel mRecipe;

        private string recipeName;

        public string RecipeName
        {
            get { return recipeName; }
            set { SetProperty(ref recipeName, value); }
        }

        private string preparationTime;

        public string PreparationTime
        {
            get { return preparationTime; }
            set { SetProperty(ref preparationTime, value); }
        }

        private string recipeText;

        public string RecipeText
        {
            get { return recipeText; }
            set { SetProperty(ref recipeText, value); }
        }


        private ObservableCollection<FoodCategoryModel> categories;

        public ObservableCollection<FoodCategoryModel> Categories
        {
            get { return categories; }
            set { SetProperty(ref categories, value); }
        }

        private int selectedCategory;

        public int SelectedCategory
        {
            get { return selectedCategory; }
            set { SetProperty(ref selectedCategory, value); }
        }


        private ObservableCollection<CuisineModel> cuisines;

        public ObservableCollection<CuisineModel> Cuisines
        {
            get { return cuisines; }
            set { SetProperty(ref cuisines, value); }
        }
        private int selectedCuisine;

        public int SelectedCuisine
        {
            get { return selectedCuisine; }
            set { SetProperty(ref selectedCuisine, value); }
        }

        private ObservableCollection<CaloricityModel> caloricities;

        public ObservableCollection<CaloricityModel> Caloricities
        {
            get { return caloricities; }
            set { SetProperty(ref caloricities, value); }
        }
        private int selectedCaloricity;

        public int SelectedCaloricity
        {
            get { return selectedCaloricity; }
            set { SetProperty(ref selectedCaloricity, value); }
        }

        public string CategoryName { get; set; }

        public string CuisineName { get; set; }

        public string CaloryName { get; set; }

        private ObservableCollection<PhotoModel> photos;

        public ObservableCollection<PhotoModel> Photos
        {
            get { return photos; }
            set { SetProperty(ref photos, value); }
        }

        #region Wizzard Indicator


        private string nextBtnIcon;

        public string NextBtnIcon
        {
            get { return nextBtnIcon; }
            set { SetProperty(ref nextBtnIcon, value); }
        }

        private string backBtnIcon;

        public string BackBtnIcon
        {
            get { return backBtnIcon; }
            set { SetProperty(ref backBtnIcon, value); }
        }


        private bool stepOneContainer;

        public bool StepOneContainer
        {
            get { return stepOneContainer; }
            set { SetProperty(ref stepOneContainer, value); }
        }

        private bool stepTwoContainer;

        public bool StepTwoContainer
        {
            get { return stepTwoContainer; }
            set { SetProperty(ref stepTwoContainer, value); }
        }

        private bool stepThreeContainer;

        public bool StepThreeContainer
        {
            get { return stepThreeContainer; }
            set { SetProperty(ref stepThreeContainer, value); }
        }

        private Color secondStepIndicatorColor;

        public Color SecondStepIndicatorColor
        {
            get { return secondStepIndicatorColor; }
            set { SetProperty(ref secondStepIndicatorColor, value); }
        }

        private Color secondStepIndicatorTextColor;

        public Color SecondStepIndicatorTextColor
        {
            get { return secondStepIndicatorTextColor; }
            set { SetProperty(ref secondStepIndicatorTextColor, value); }
        }

        private Color secondStepLineColor;

        public Color SecondStepLineColor
        {
            get { return secondStepLineColor; }
            set { SetProperty(ref secondStepLineColor, value); }
        }

        private Color thirdStepIndicatorColor;

        public Color ThirdStepIndicatorColor
        {
            get { return thirdStepIndicatorColor; }
            set { SetProperty(ref thirdStepIndicatorColor, value); }
        }

        private Color thirdStepIndicatorTextColor;

        public Color ThirdStepIndicatorTextColor
        {
            get { return thirdStepIndicatorTextColor; }
            set { SetProperty(ref thirdStepIndicatorTextColor, value); }
        }

        private Color thirdStepLineColor;

        public Color ThirdStepLineColor
        {
            get { return thirdStepLineColor; }
            set { SetProperty(ref thirdStepLineColor, value); }
        }


        private int mPageNumber;



        #endregion

        private RecipeCommonDataModel mCommonData;
        private List<Stream> PhotoStreams;

        public Command NextStepCommand { get; }

        public Command PreviousStepCommand { get; }

        public Command SaveCommand { get; }

        public Command CancelCommand { get; }

        public Command AddImageCommand { get; }

        public Command DeleteImageCommand { get; }

        public Command ViewImageCommand { get; }



        public EditRecipeViewModel(RecipeDataModel recipe)
        {
            mRecipe = recipe;
            RecipeName = recipe.Name;
            PreparationTime = recipe.PreparationTime.ToString();
            RecipeText = recipe.RecipeText;            

            InitData();

            mDeletedPhotos = new List<PhotoModel>();            
            DataModel = new PostRecipeModel();
            Photos = new ObservableCollection<PhotoModel>(recipe.Photos);

            BackBtnIcon = "cancel";
            NextBtnIcon = "next";

            //Init Wizard
            StepOneContainer = true;
            StepTwoContainer = false;
            StepThreeContainer = false;

            SecondStepIndicatorColor = Color.White;
            SecondStepIndicatorTextColor = Color.Gray;
            SecondStepLineColor = Color.White;

            ThirdStepIndicatorColor = Color.White;
            ThirdStepIndicatorTextColor = Color.Gray;
            ThirdStepLineColor = Color.White;
            //

            

            NextStepCommand = new Command((x) => NextStep(x));
            PreviousStepCommand = new Command((x) => PreviousStep(x));

            SaveCommand = new Command(() => EditRecipe());
            CancelCommand = new Command(() => Cancel());
            AddImageCommand = new Command((x) => AddImage(x));
            DeleteImageCommand = new Command((x) => DeleteImage(x));
            ViewImageCommand = new Command((x) => ViewImage(x));

            PhotoStreams = new List<Stream>();
            mPageNumber = 0;
        }

        private async void EditRecipe()
        {            
            DataModel.CategoryId = mCommonData.Categories.ElementAt(SelectedCategory).CategoryId;
            DataModel.CuisineId = mCommonData.Cuisines.ElementAt(SelectedCuisine).CuisineId;
            DataModel.CaloricityId = mCommonData.Caloricities.ElementAt(SelectedCaloricity).CaloricityId;
            DataModel.Name = RecipeName;
            DataModel.PreparationTime = int.Parse(PreparationTime);
            DataModel.RecipeText = RecipeText;
            DataModel.Photos = Photos.ToList();
            foreach (var photo in mDeletedPhotos)
            {
                photo.IsDeleted = true;
                DataModel.Photos.Add(photo);
            }
            Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            bool result = await RecipeDataService.EditRecipe(DataModel, mRecipe.RecipeId, LocalDataSecureStorage.GetToken());

            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

            if (result)
                MessagingCenter.Send(this, MessageCenterKeys.EDITED);
            else
                await App.Current.MainPage.DisplayAlert("Obveštenje", "Greška prilikom ažuriranja recepta.", "U redu");

        }



        private async void InitData()
        {


            Device.BeginInvokeOnMainThread(() => Dialogs.Show());

            var realm = Realm.GetInstance();
            if (realm.All<FoodCategoryModel>().Any() && realm.All<CuisineModel>().Any() && realm.All<CaloricityModel>().Any())
            {
                Categories = new ObservableCollection<FoodCategoryModel>(realm.All<FoodCategoryModel>());
                Cuisines = new ObservableCollection<CuisineModel>(realm.All<CuisineModel>());
                Caloricities = new ObservableCollection<CaloricityModel>(realm.All<CaloricityModel>());

                mCommonData = new RecipeCommonDataModel
                {
                    Caloricities = Caloricities.ToList(),
                    Categories = Categories.ToList(),
                    Cuisines = Cuisines.ToList()
                };
            }
            else
            {
                RecipeCommonDataModel commonData = await RecipeDataService.GetRecipeCommonDate();

                realm.Write(() =>
                {

                    foreach (var item in commonData.Categories)
                    {
                        realm.Add(item);
                    }

                    foreach (var item in commonData.Cuisines)
                    {
                        realm.Add(item);
                    }

                    foreach (var item in commonData.Caloricities)
                    {
                        realm.Add(item);
                    }
                });

                Categories = new ObservableCollection<FoodCategoryModel>(commonData.Categories);
                Cuisines = new ObservableCollection<CuisineModel>(commonData.Cuisines);
                Caloricities = new ObservableCollection<CaloricityModel>(commonData.Caloricities);

                mCommonData = commonData;

            }

           

            SelectedCategory = Categories.IndexOf(Categories.FirstOrDefault(x => x.CategoryId == mRecipe.CategoryId));
            OnPropertyChanged("SelectedCategory");

            SelectedCuisine = Cuisines.IndexOf(Cuisines.FirstOrDefault(x => x.CuisineId == mRecipe.CuisineId)); ;
            OnPropertyChanged("SelectedCuisine");

            SelectedCaloricity = Caloricities.IndexOf(Caloricities.FirstOrDefault(x => x.CaloricityId == mRecipe.CaloricityId));
            OnPropertyChanged("SelectedCaloricity");

            CategoryName = mCommonData.Categories.ElementAt(SelectedCategory)?.CategoryName;
            CuisineName = mCommonData.Cuisines.ElementAt(SelectedCuisine)?.CuisineName;
            CaloryName = mCommonData.Caloricities.ElementAt(SelectedCaloricity)?.Name;

            Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

        }

        private async void Cancel()
        {
     
                HomeMasterDetailPage home = App.Current.MainPage as HomeMasterDetailPage;
                await home.Detail.Navigation.PopAsync();            
        }

        private async void AddImage(object sender)
        {
            Utils.ButtonPress(sender);

            try
            {
                string action = await App.Current.MainPage.DisplayActionSheet("Dodavanje slike: Izaberite sliku pomoću?", "Otkaži", null, "Kamera", "Galerija");
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                await CrossMedia.Current.Initialize();

                string photoName = Guid.NewGuid().ToString() + ".jpg";

                MediaFile file = null;
                if (action.Equals("Kamera"))
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                        return;
                    }


                    file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = photoName,
                        SaveToAlbum = false,
                        CompressionQuality = 50

                    });
                }
                else
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await App.Current.MainPage.DisplayAlert("Info", "Ne možete izabrati sliku.", "U redu");
                        Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                        return;
                    }

                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        CompressionQuality = 50

                    });

                }

                if (file == null)
                    return;


                var temp = Photos;
                temp.Add(new PhotoModel
                {
                    Url = file.Path,
                    IsAdded = true,
                    PhotoStream = file.GetStream(),
                    Name = photoName
                    
                });
                Photos = new ObservableCollection<PhotoModel>(temp);
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                file.Dispose();
            }
            catch (Exception ex)
            {

                int i = 2;
            }
        }

        private async void DeleteImage(object photo)
        {
            bool res = await App.Current.MainPage.DisplayAlert("Uklanjanje slike", "Da li želite da uklonite sliku?", "Da", "Ne");

            if (res)
            {
                var temp = Photos;
                var photoToRemove = photo as PhotoModel;
                if (photoToRemove != null)
                {
                    temp.Remove(photoToRemove);
                    if (!photoToRemove.IsAdded)
                        mDeletedPhotos.Add(photoToRemove);
                }
                Photos = new ObservableCollection<PhotoModel>(temp);
            }
        }

        private async void ViewImage(object photo)
        {
            var photoModel = photo as PhotoModel;

            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PushModalAsync(new ImageViewPage(photoModel.Url));
        }

        private async void NextStep(object sender)
        {
            Utils.ButtonPress(sender);

            switch (mPageNumber)
            {
                case 0:
                    bool isInvalid = false;
                    if (string.IsNullOrEmpty(RecipeName))
                    {
                        RecipeName = "";
                        isInvalid = true;
                    }
                    if (string.IsNullOrEmpty(PreparationTime))
                    {
                        isInvalid = true;
                        PreparationTime = "";
                    }
                    else
                    {
                        int temp = 0;
                        int.TryParse(PreparationTime, out temp);
                        if (temp == 0)
                        {
                            PreparationTime = "";
                            isInvalid = true;
                        }                       
                    }

                    if (!isInvalid)
                        mPageNumber++;
                    break;
                case 1:
                    if (!string.IsNullOrEmpty(RecipeText))
                        mPageNumber++;
                    else
                    {
                        RecipeText = "";
                    }
                    break;
                case 2:
                    bool result = await App.Current.MainPage.DisplayAlert("Obvestenje", "Da li ste sigurni da zelite da sacuvate recept?", "Da", "Ne");
                    if (result)
                        EditRecipe();
                    break;
            }
            SetPage();            
            
        }

        private async void PreviousStep(object sender)
        {
            Utils.ButtonPress(sender);
            if(mPageNumber > 0)
                mPageNumber--;
            else if (mPageNumber == 0)
            {
                bool result = await App.Current.MainPage.DisplayAlert("Obvestenje", "Da li ste sigurni da zelite da prekinete proces dodavanja novog recepta?", "Da", "Ne");
                if (result)
                    Cancel();
            }
            SetPage();
        }

        private void SetPage()
        {
            StepOneContainer = false;
            StepTwoContainer = false;
            StepThreeContainer = false;
            SecondStepIndicatorColor = Color.White;
            SecondStepIndicatorTextColor = Color.Gray;
            SecondStepLineColor = Color.White;
            ThirdStepIndicatorColor = Color.White;
            ThirdStepIndicatorTextColor = Color.Gray;
            ThirdStepLineColor = Color.White;

            if (mPageNumber == 2)
            {
                NextBtnIcon = "save";
                BackBtnIcon = "back1";
                StepThreeContainer = true;
                SecondStepIndicatorColor = Color.FromHex(MyColors.GREEN);
                SecondStepIndicatorTextColor = Color.White;
                SecondStepLineColor = Color.FromHex(MyColors.GREEN);
                ThirdStepIndicatorColor = Color.FromHex(MyColors.GREEN);
                ThirdStepIndicatorTextColor = Color.White;
                ThirdStepLineColor = Color.FromHex(MyColors.GREEN);
            }
            else if (mPageNumber == 1)
            {
                NextBtnIcon = "next";
                BackBtnIcon = "back1";
                StepTwoContainer = true;
                SecondStepIndicatorColor = Color.FromHex(MyColors.GREEN);
                SecondStepIndicatorTextColor = Color.White;
                SecondStepLineColor = Color.FromHex(MyColors.GREEN);

            }
            else if (mPageNumber == 0)
            {
                NextBtnIcon = "next";
                BackBtnIcon = "cancel";
                StepOneContainer = true;
            }
        }
    }
}
