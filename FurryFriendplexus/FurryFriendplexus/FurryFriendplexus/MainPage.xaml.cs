using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FurryFriendplexus.LocalDB;

namespace FurryFriendplexus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        // objekt databáze pro uživatele
        private LocalUserDB LUDB = new LocalUserDB();

        // Objekt databáze pro záznamy a jejich jména a hodnocení
        private LocalRatingDB LRDB = new LocalRatingDB();
        public MainPage()
        {
            InitializeComponent();
        }

        // funkce kontrolující zda-li, při zjevení této stránky na obrazovce zařízení,  je uživatel přihlášený
        private void MasterDetailPage_Appearing(object sender, EventArgs e)
        {
            // pokud uživatel není přihlášený, převedeme ho na stránku pro přihlášení
            if (!LUDB.LookAtLocalLogin())
            {
                for (int i = 0; i < Navigation.ModalStack.Count(); i++)
                {
                    Navigation.PopModalAsync();
                }
                Navigation.PushModalAsync(new Login());
            }
            // poud uživatel nemá svůj záznam, převede se na stránku kde si zadá svůj záznam
            else if (!LRDB.HaveUserRecord(LUDB.WhoLogged()))
            {
                for (int i = 0; i < Navigation.ModalStack.Count(); i++)
                {
                    Navigation.PopModalAsync();
                }
                Navigation.PushModalAsync(new NewRating(true));
            }
            // aktualizuje jméno v bočním meniu aktuálního přihlášenýho uživatele
            (Master as SideMenu).Update_name();
            // oznámí svím podstránkám, že byl zjeven na obrazovce
            (((Detail as NavigationPage).RootPage as Home).Children[1] as Rating).Start_Getting_Rating();
            (((Detail as NavigationPage).RootPage as Home).Children[2] as NewGroup).Actualize_NameList();

        }
    }
}
