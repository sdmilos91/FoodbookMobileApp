using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
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
    public class RegisterPageViewModel : BaseViewModel
    {

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

        public Command RegisterCommand { get; }
        public Command AddImageCommand { get; }
        public Command ViewImageCommand { get; }
        public Command DeleteImageCommand { get; }

        public RegisterPageViewModel()
        {
            RegisterModel = new PostRegisterModel();
            RegisterModel.PhotoUrl = "addPhotoHolder.png";
            PhotoPicked = false;

            RegisterCommand = new Command(() => Register());
            AddImageCommand = new Command((x) => AddImage(x));
            ViewImageCommand = new Command((x) => ViewImage(x));
            DeleteImageCommand = new Command((x) => DeleteImage(x));
        }

        private async void Register()
        {
          

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

            if (string.IsNullOrEmpty(RegisterModel.Email))
            {
                RegisterModel.Email = "";
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RegisterModel.Password))
            {
                RegisterModel.Password = "";
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RegisterModel.ConfirmPassword))
            {
                RegisterModel.ConfirmPassword = "";
                isFormValid = false;
            }

            if (string.IsNullOrEmpty(RegisterModel.Biography))
            {
                RegisterModel.Biography = "";
                isFormValid = false;
            }

            if (isFormValid && Utils.IsEmailValid(RegisterModel.Email))
            {
                if (RegisterModel.PhotoUrl.Equals("addPhotoHolder.png"))
                {
                    RegisterModel.PhotoUrl = null;
                }

                ShowDialog();
                bool res = await AccountDataService.RegisterUser(registerModel);
                HideDialog();

                Page masterPage = App.Current.MainPage;
                if (res)
                {
                    
                    await masterPage.DisplayAlert("Obaveštenje", "Uspešno ste se registrovali. Možete se prijaviti sa registrovanim nalogom.", "U redu");
                    await masterPage.Navigation.PopAsync();
                }
                else
                {
                    await masterPage.DisplayAlert("Obaveštenje", "Greška Prilikom registracije.", "U redu");
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
                ShowDialog();
                await CrossMedia.Current.Initialize();

                string photoName = Guid.NewGuid().ToString() + ".jpg";

                MediaFile file = null;
                if (action.Equals("Kamera"))
                {
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        HideDialog();
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
                        HideDialog();
                        return;
                    }


                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {                        
                        CompressionQuality = 50

                    });

                    
                }

                if (file == null)
                {
                    HideDialog();
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

                HideDialog();
                file.Dispose();
                
            }
            catch (Exception ex)
            {

                HideDialog();
            }
        }

        private async void DeleteImage(object sender)
        {

            Utils.ButtonPress(sender);

            bool res = await App.Current.MainPage.DisplayAlert("Uklanjanje slike", "Da li želite da uklonite sliku?", "Da", "Ne");

            if (res)
            {
                RegisterModel.Photo = null;
                RegisterModel.PhotoUrl = "addPhotoHolder.png";
                OnPropertyChanged("RegisterModel");
                PhotoPicked = false;
                OnPropertyChanged("PhotoPicked");
            }
        }

        private async void ViewImage(object sender)
        {
            Utils.ButtonPress(sender);

            await App.Current.MainPage.Navigation.PushModalAsync(new ImageViewPage(RegisterModel.PhotoUrl));
        }
    }
}
