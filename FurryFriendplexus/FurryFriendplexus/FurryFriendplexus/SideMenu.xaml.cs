﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FurryFriendplexus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenu : ContentPage
    {
        public SideMenu()
        {
            InitializeComponent();
        }

        private void Go_Home(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new MainPage());

        }
    }
}