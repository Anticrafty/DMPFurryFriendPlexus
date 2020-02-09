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
    public partial class SideMenu : ContentPage
    {
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();

        public SideMenu()
        {
            InitializeComponent();
        }

        // Přepínání na Home Stránku
        private void Go_Home(object sender, EventArgs e)
        {

            Make_Go_Home();

        }

        private void LogOut_Click(object sender, EventArgs e)
        {

            Make_Go_Home();
            LUDB.LogOutHim();
            Navigation.PushModalAsync(new Login());
        }
        public void Make_Go_Home()
        {
            for (int i = 0; i < Navigation.ModalStack.Count(); i++)
            {
                Navigation.PopModalAsync();
            }
        }
        public void Update_name()
        {
            UsernameV.Text = LUDB.WhoLogged().Nickname;
        }
    }
}