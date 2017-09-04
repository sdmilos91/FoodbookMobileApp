using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
using Foodbook.MobileApp.Tools;
using Foodbook.Pages;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.ViewModels
{
    public class EditUserDetailsPageViewModel : BaseViewModel
    {
        private ResponseCookModel mCook;

        private PostRegisterModel registerModel;

        public PostRegisterModel RegisterModel
        {
            get { return registerModel; }
            set { SetProperty(ref registerModel, value); }
        }

        private bool photoPicked;

        public bool PhotoPicked
        {
            get { return photoPicked; }
            set { SetProperty(ref photoPicked, value); }
        }


        public Command EditCommand { get; }
        public Command AddImageCommand { get; }
        public Command ViewImageCommand { get; }
        public Command DeleteImageCommand { get; }

        public EditUserDetailsPageViewModel(ResponseCookModel cook)
        {
            mCook = cook;
            RegisterModel = new PostRegisterModel
            {
                Biography = cook.Biography,
                Email = LocalDataSecureStorage.GetEmail(),
                FirstName = cook.FirstName,
                LastName = cook.LastName,
                PhotoUrl = cook.PhotoUrl
            };

            if (string.IsNullOrEmpty(cook.PhotoUrl))
            {
                PhotoPicked = false;
                RegisterModel.PhotoUrl = "addPhotoHolder";
            }
            else
            {
                PhotoPicked = true;
            }
            

            EditCommand = new Command(() => Edit());
            AddImageCommand = new Command((x) => AddImage(x));
            ViewImageCommand = new Command((x) => ViewImage(x));
            DeleteImageCommand = new Command((x) => DeleteImage(x));
        }

        private async void Edit()
        {
            if (RegisterModel.PhotoUrl.Equals("addPhotoHolder"))
            {
                RegisterModel.PhotoUrl = null;
            }

            bool isFormValid = true;

            if (string.IsNullOrEmpty(RegisterModel.FirstName))
            {
                RegisterModel.FirstName = "";
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RegisterModel.LastName))
            {
                RegisterModel.LastName = "";
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RegisterModel.Biography))
            {
                RegisterModel.Biography = "";
                isFormValid = false;
            }

            if (isFormValid && Utils.IsEmailValid(RegisterModel.Email))
            {
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                bool res = await CookDataService.EditCook(registerModel, mCook.CookId, LocalDataSecureStorage.GetToken());
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

                MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
                if (res)
                {                    
                    mCook.Biography = RegisterModel.Biography;
                    mCook.FirstName = RegisterModel.FirstName;
                    mCook.LastName = RegisterModel.LastName;
                    mCook.FullName = string.Join(" ", RegisterModel.FirstName, RegisterModel.LastName);
                    mCook.PhotoUrl = RegisterModel.PhotoUrl;
                    MessagingCenter.Send(this, "USER_EDIITED", mCook);

                    await masterPage.DisplayAlert("Obaveštenje", "Uspešno ste ažurirali profil.", "U redu");
                    await masterPage.Detail.Navigation.PopAsync();
                }
                else
                {
                    await masterPage.DisplayAlert("Obaveštenje", "Greška prilikom ažuriranja.", "U redu");
                }
            }
            else
            {
                OnPropertyChanged("RegisterModel");
            }

            
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
                else if (action.Equals("Galerija"))
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
                {
                    Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                    return;
                }


                RegisterModel.Photo = new PhotoModel
                {
                    Url = file.Path,
                    IsAdded = true,
                    PhotoStream = file.GetStream(),
                    Name = photoName

                };

                RegisterModel.PhotoUrl = file.Path;
                OnPropertyChanged("RegisterModel");
                PhotoPicked = true;

                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
                file.Dispose();
                
            }
            catch (Exception ex)
            {

                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());
            }
        }

        private async void DeleteImage(object sender)
        {

            Utils.ButtonPress(sender);

            bool res = await App.Current.MainPage.DisplayAlert("Uklanjanje slike", "Da li želite da uklonite sliku?", "Da", "Ne");

            if (res)
            {
                RegisterModel.Photo = null;
                if (RegisterModel.PhotoUrl.Equals(mCook.PhotoUrl))
                {
                    RegisterModel.RemovedPhotoUrl = RegisterModel.PhotoUrl;
                    RegisterModel.PhotoUrl = "addPhotoHolder";
                    OnPropertyChanged("RegisterModel");
                }

                PhotoPicked = false;
                OnPropertyChanged("PhotoPicked");
            }
        }

        private async void ViewImage(object sender)
        {
            Utils.ButtonPress(sender);

            MasterDetailPage masterPage = App.Current.MainPage as MasterDetailPage;
            await masterPage.Detail.Navigation.PushModalAsync(new ImageViewPage(RegisterModel.PhotoUrl));
        }
    }
}
