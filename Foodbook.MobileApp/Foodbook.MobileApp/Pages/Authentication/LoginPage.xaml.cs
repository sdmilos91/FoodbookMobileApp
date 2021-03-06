﻿using Foodbook.MobileApp.Tools;
using Foodbook.MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Foodbook.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new LoginViewModel();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}