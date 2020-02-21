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

        // funkce tlačítka Přepínání na Home Stránku
        private void Go_Home(object sender, EventArgs e)
        {

            Make_Go_Home();

        }
        // funkce na odhlašování 
        private void LogOut_Click(object sender, EventArgs e)
        {
            // přepni se na hlavní stránku            
            Make_Go_Home();
            // Odhlaš uživatele
            LUDB.LogOutHim();
            // Přepni na stránku přihlášení
            Navigation.PushModalAsync(new Login());
        }
        // funkce Přepínání na Home Stránku
        public void Make_Go_Home()
        {
            // zavři všechny stránky, který jsou před hlavní stránkou
            for (int i = 0; i < Navigation.ModalStack.Count(); i++)
            {
                Navigation.PopModalAsync();
            }
        }
        // funkce na aktualizování jména Uživatele
        public void Update_name()
        {
            UsernameV.Text = LUDB.WhoLogged().Nickname;
        }
    }
}