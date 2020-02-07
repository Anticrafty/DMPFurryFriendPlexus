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
        LocalDB.LocalDB LDB = new LocalDB.LocalDB();

        public SideMenu()
        {
            InitializeComponent();
        }

        // Přepínání na Home Stránku
        private void Go_Home(object sender, EventArgs e)
        {

            for (int i = 0; i < Navigation.ModalStack.Count(); i++)
            {
                Navigation.PopModalAsync();
            }

        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            LDB.LogOutHim();
            Navigation.PushModalAsync(new Login());
        }

        public void Update_name()
        {
            UsernameV.Text = LDB.WhoLogged();
        }
    }
}