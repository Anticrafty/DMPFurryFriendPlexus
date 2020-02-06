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
    public partial class Home : TabbedPage
    {
        public Home()
        {
            InitializeComponent();
            // Nastavení Defaultní stránky
            CurrentPage = Children[1];
            //https://stackoverflow.com/questions/42863886/how-set-tab-2-in-a-xamarin-forms-tabbed-page-as-the-default-tab-on-startup
        }

        // Měnění nadpisu vedle menu
        private void Page_Changing(object sender, EventArgs e)
        {
            this.Title = CurrentPage.Title;
        }
    }
}