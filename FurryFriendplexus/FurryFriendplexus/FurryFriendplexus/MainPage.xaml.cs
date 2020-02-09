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
        private LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public MainPage()
        {
            InitializeComponent();
        }


        private void MasterDetailPage_Appearing(object sender, EventArgs e)
        {
            if (!LUDB.LookAtLocalLogin())
            {
                for (int i = 0; i < Navigation.ModalStack.Count(); i++)
                {
                    Navigation.PopModalAsync();
                }
                Navigation.PushModalAsync(new Login());
            }
            (Master as SideMenu).Update_name();
            (((Detail as NavigationPage).RootPage as Home).Children[1] as Rating).Start_Getting_Rating();

        }
    }
}
