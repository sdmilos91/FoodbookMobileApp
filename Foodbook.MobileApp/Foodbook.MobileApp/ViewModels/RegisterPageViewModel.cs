﻿using Foodbook.MobileApp.Data.Models;
using Foodbook.MobileApp.Data.Services;
using Foodbook.MobileApp.Helpers;
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


        public Command RegisterCommand { get; set; }

        public RegisterPageViewModel()
        {
            RegisterModel = new PostRegisterModel();

            RegisterCommand = new Command(() => Register());
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
                Device.BeginInvokeOnMainThread(() => Dialogs.Show());
                bool res = await AccountDataService.RegisterUser(registerModel);
                Device.BeginInvokeOnMainThread(() => Dialogs.Hide());

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
    }
}