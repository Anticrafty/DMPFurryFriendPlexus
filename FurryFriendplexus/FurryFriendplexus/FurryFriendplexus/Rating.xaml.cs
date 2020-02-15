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
        LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();

        int NumberToRate = 0;
        int NumberLastRated = 0;
        List<Classes.Record> ToRate = new List<Classes.Record>();
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
            NumberDisplay.Text = RatingSlider.Value.ToString() + "%";
        }

        // Přepínání na Stránku pro nový záznam
        private void Change_To_NewRating(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewRating(false));
            // https://stackoverflow.com/questions/32048249/how-to-move-from-one-page-to-another-from-button-click-in-xamarin-forms
        }
        public void Start_Getting_Rating()
        {
            Get_New_Rating();
            Next_Rating();
        }
        private void Get_New_Rating()
        {
            if (LUDB.WhoLogged().Id != -1)
            {
                ToRate = LRDB.GetStartingRecords(LUDB.WhoLogged().Id);
                if (ToRate.Count() != 0)
                {
                    int countOfRecord = 0;
                    List<int> countersOfRecords = new List<int>();
                    foreach (Classes.Record record in ToRate)
                    {
                        if (record.LinkedUserID == LUDB.WhoLogged().Id)
                        {
                            countersOfRecords.Add(countOfRecord);
                        }
                        countOfRecord++;
                    }
                    countersOfRecords.Reverse();
                    //https://www.geeksforgeeks.org/different-ways-to-sort-an-array-in-descending-order-in-c-sharp/
                    foreach (int recordID in countersOfRecords)
                    {
                        ToRate.Remove(ToRate[recordID]);
                    };
                }
                NumberToRate = ToRate.Count();
                NumberLastRated = 0;
            }   
        }
        private void Next_Rating()
        {
            Names_Stack.Children.Clear();
            Race.Text = "";
            Confirmer.IsEnabled = false;
            if (ToRate.Count() != 0 && NumberLastRated != NumberToRate)
            {
                Race.Text = ToRate[NumberLastRated].Race;
                foreach (Classes.Namies namies in LRDB.GetNames(ToRate[NumberLastRated].Id))
                {
                    Label Namie = new Label
                    {
                        Text = namies.Name,
                        FontSize = 17,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    Names_Stack.Children.Add(Namie);
                }
                Confirmer.IsEnabled = true;
            }
            if (ToRate.Count() == 0)
            {
                Race.Text = "Žádné nové záznamy";
            }
            else if (NumberLastRated == NumberToRate)
            {
                Start_Getting_Rating();
            }
            
        }

        private void Rating_Appearing(object sender, EventArgs e)
        {
            Start_Getting_Rating();
        }

        private void Confirmer_Clicked(object sender, EventArgs e)
        {
            Classes.Ratinging NewRatinging = new Classes.Ratinging
            {
                Rate = int.Parse(RatingSlider.Value.ToString()),
                RecordID = ToRate[NumberLastRated].Id,
                RaterUserID = LUDB.WhoLogged().Id
            };
            LRDB.SaveRating(NewRatinging);
            NumberLastRated++;
            Next_Rating();
        }
    }
}