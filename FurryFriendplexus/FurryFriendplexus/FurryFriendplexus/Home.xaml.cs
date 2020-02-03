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
        }

        private void Page_Changing(object sender, EventArgs e)
        {
            this.Title = CurrentPage.Title;
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {

        }
    }
}