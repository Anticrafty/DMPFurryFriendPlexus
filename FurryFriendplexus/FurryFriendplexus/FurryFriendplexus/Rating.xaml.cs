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
    public partial class Rating : ContentPage
    {
        public Rating()
        {
            InitializeComponent();
        }

        // Určení posuvníku použe na cvelá čísla a aktualizování jeho číselníku
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1.0);

            RatingSlider.Value = newStep * 1.0;
            // https://forums.xamarin.com/discussion/22473/can-you-limit-a-slider-to-only-allow-integer-values-hopefully-snapping-to-the-next-integer
            Ciselnik.Text = RatingSlider.Value.ToString() + "%";
        }

        // Přepínání na Stránku pro nový záznam
        private void Change_To_NewRating(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewRating());
            // https://stackoverflow.com/questions/32048249/how-to-move-from-one-page-to-another-from-button-click-in-xamarin-forms
        }
    }
}