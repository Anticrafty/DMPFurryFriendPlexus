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
        private LocalDB.LocalDB LDB;
        public MainPage()
        {
            InitializeComponent();

            LDB = new LocalDB.LocalDB();

            bool beenLoged = LDB.LookAtLocalLogin();

            if (beenLoged == false)
            {
                Navigation.PushModalAsync(new EndApp());
            }

        }
    }
}
