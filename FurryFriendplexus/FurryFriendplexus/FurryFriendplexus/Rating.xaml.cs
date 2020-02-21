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
        // Objekt databáze pro záznamy a jejich jména a hodnocení
        LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();
        // objekt databáze pro uživatele
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();

        // počet záznamů, které jsou potřeba ohodnotit
        int NumberToRate = 0;
        // ID záznamu, kteý byl posledná ohodnocen
        int NumberLastRated = 0;
        // seznam záznamů, který uživatel potřebuju ohodnotit
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
        // funkce nna aktualizování této stránky
        public void Start_Getting_Rating()
        {
            Get_New_Rating();
            Next_Rating();
        }
        // funkce na získání nových záznamů na hodnocení
        private void Get_New_Rating()
        {
            // zkontrolovat jestli je někdo přihlášený
            if (LUDB.WhoLogged().Id != -1)
            {
                // získej zátnamy, který aktuání uživatel ještě nehodnotil
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
                    // zadej je do seznamu, záznamů, které jsou potřeba ohodnotit
                    foreach (int recordID in countersOfRecords)
                    {
                        ToRate.Remove(ToRate[recordID]);
                    };
                }
                // spočítej kolik jich je potřeba ohodnotit a vynuluj ID posledního ohodnocenýho
                NumberToRate = ToRate.Count();
                NumberLastRated = 0;
            }   
        }
        // funkce na ukázání nového záznamu na hodnocení
        private void Next_Rating()
        {
            // smaž ukázání pro předchozí záznam
            Names_Stack.Children.Clear();
            Race.Text = "";
            RatingSlider.Value = 0;
            Confirmer.IsEnabled = false;
            // pokud je ještě co hodnotit
            if (ToRate.Count() != 0 && NumberLastRated != NumberToRate)
            { 
                // tak to zobraz
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
                RatingSlider.Value = 0;
            }
            // pokud neexistují, žádné nové záznamy
            if (ToRate.Count() == 0)
            {
                // tak to oznam uživately
                Race.Text = "Žádné nové záznamy";
            }
            // pokud jsi vyčerpal seznam, záznamů, na zhodnocení
            else if (NumberLastRated == NumberToRate)
            {
                // tak si zkus, jestli nejsou potřeba další záznamy zhodnotit
                Start_Getting_Rating();
            }
            
        }
        // Pokud se zjeví tato stránka
        private void Rating_Appearing(object sender, EventArgs e)
        {
            // tak si zkus, jestli nejsou potřeba záznamy zhodnotit
            Start_Getting_Rating();
        }
        // Funkce tlačítka na potvrzování rozhodnutí hodnocení 
        private void Confirmer_Clicked(object sender, EventArgs e)
        {
            // zabalí a pošle hodnocení
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