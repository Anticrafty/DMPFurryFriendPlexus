using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FurryFriendplexus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        LocalDB.LocalDB LDB = new LocalDB.LocalDB();
        public Login()
        {
            InitializeComponent();
            if (LDB.LookAtLocalLogin())
            {
                Navigation.PopModalAsync();
            }

        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            Classes.Users user = new Classes.Users { Nickname = UsernameE.Text, Password = PasswordE.Text };
            if(LDB.LoginHim(user))
            {
                Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("", "Jmeno nebo heslo jsou neplatné", "OK");
                PasswordE.Text = "";
            }
            
        }
        private void Registr_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Registration());
        }
    }
}