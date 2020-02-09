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
    public partial class Registration : ContentPage
    {

        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public Registration()
        {
            InitializeComponent();
        }
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            bool AllOk = true;
            if (LUDB.TryUsername(UsernameE.Text))
            {
                UsernameE.Text = "";
                DisplayAlert("", "Toto jméno už je zabrané", "OK");
                // Source: https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/pop-ups
                AllOk = false;
            } else if (UsernameE.Text == null )
            {
                DisplayAlert("", "Musíte mít nějaké Uživatelské jméno", "OK");
                AllOk = false;
            }
           
             if (PasswordE.Text == null )
            {
                DisplayAlert("", "Heslo je moc krátké", "OK");
                AllOk = false;
            }  
            else if (PasswordE.Text.Length < 8)
            {
                DisplayAlert("", "Heslo je moc krátké", "OK");
                AllOk = false;
            }
            else if (PasswordE.Text != PasswordTest.Text)
            {
                PasswordE.Text = "";
                PasswordTest.Text = "";
                DisplayAlert("", "Hesla se nehodují", "OK");
                AllOk = false;
            }

            if (AllOk)
            {
                Classes.Users user = new Classes.Users { Nickname = UsernameE.Text, Password = PasswordE.Text };
                LUDB.RegisterHim(user);

                DisplayAlert("", "Byl jste zaregistrován", "OK");
                Navigation.PopModalAsync();
            }
        }
        private void Login_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}