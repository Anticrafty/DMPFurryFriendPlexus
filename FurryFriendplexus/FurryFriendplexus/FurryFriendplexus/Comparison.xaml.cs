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
    public partial class Comparison : ContentPage
    {
        // objekt pro všechny zaznamy pro hodnocení
        List<Classes.Record> Selected = new List<Classes.Record>();
        // úroveň přatel pro vyprání dalších záznamů na hodnocenní
        int LevelOfSearch = 0;
        // Objekt databáze pro záznamy a jejich jména a hodnocení
        LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();
        // objekt databáze pro uživatele
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        // list ID uživaelů, který byli odškrtnutý ze seznamu
        List<int> Hidden = new List<int>();

        public Comparison( List<Classes.Namies> selected, int levelOfSearch)
        {

            InitializeComponent();
            // předávání upotřebné urovně přátelství 
            LevelOfSearch = levelOfSearch;
            // přidávání přihlášeného uživatele a zadaných záznamů do seznamu záznamů pro hodnocení
            bool UserInThere = false;
            Selected.Add(LRDB.GetUsersRecord(LUDB.WhoLogged().Id));
            foreach (Classes.Namies Import in selected)
            {
                Classes.Record Importee = LRDB.FindRecord(Import.RecordID);
                // kontrola jestli jeden ze zadaných záznamů není přihlášený uživatel
                if (Importee.Id != LRDB.GetUsersRecord(LUDB.WhoLogged().Id).Id)
                {
                    Selected.Add(Importee);
                }
            }
            // hledánní potřebných záznamů podle zadaných parametrů a záznamů
            foreach (Classes.Namies Select in selected)
            {
                // najdi ID uživatele který koresponduje se zadaným záznamem
                int SelectedID = LRDB.GetUsersIDFromNamesID(Select.RecordID);
                // pokud takový uživatel existuje 
                if (SelectedID != -1)
                {
                    // najdi všechny záznamy tohoto uživatele, dle zadaný úrovně přáteství
                    foreach (Classes.Ratinging rated in LRDB.GetUsersRatings(SelectedID))
                    {
                        bool isVanted = false;
                        if (LevelOfSearch == 1)
                        {
                            if (rated.Rate > 20)
                            {
                                isVanted = true;
                            }
                        }
                        if (LevelOfSearch == 2)
                        {
                            if (rated.Rate > 50)
                            {
                                isVanted = true;
                            }
                        }
                        if (LevelOfSearch == 3)
                        {
                            if (rated.Rate > 70)
                            {
                                isVanted = true;
                            }
                        }
                        // pokud je již v seznamu k hodnocení, tak se nepřidá
                        foreach (Classes.Namies Selecties in selected)
                        {
                            if (Selecties.RecordID == rated.RecordID)
                            {
                                isVanted = false;
                            }
                        }
                        foreach (Classes.Record vanted in Selected)
                        {
                            if (vanted.Id == rated.RecordID)
                            {
                                isVanted = false;
                            }
                        }
                        if (isVanted)
                        {
                            Selected.Add(LRDB.FindRecord(rated.RecordID));
                        }
                    }
                }
            }
            
            foreach (Classes.Record Showing in Selected)
            {
                // pro každýho v seznamu k hodnocení se vytvoří záznam 
                Frame Peel = new Frame { Padding = new Thickness(5, 5, 5, 5), BorderColor = Color.Black };
                StackLayout Crust = new StackLayout { Orientation = StackOrientation.Horizontal };
                Button Hide = new Button { WidthRequest = 40, HeightRequest = 40, ClassId = Showing.Id.ToString(), HorizontalOptions = LayoutOptions.EndAndExpand, Text = "X", TextColor = Color.Red, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand };
                Hide.Clicked += Hide_Clicked;
                Button Unhide = new Button { WidthRequest = 40, HeightRequest = 40, ClassId = Showing.Id.ToString(), IsVisible = false, HorizontalOptions = LayoutOptions.EndAndExpand, Text = "V", TextColor = Color.Green, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand };
                Unhide.Clicked += Unhide_Clicked;
                ScrollView Shell = new ScrollView { Orientation = ScrollOrientation.Horizontal };
                StackLayout Seed = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center
                };
                Label Name = new Label
                {
                    TextColor = Color.Black,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    Text = LRDB.GetName(Showing.Id).Name
                };

                Seed.Children.Add(Name);
                // pokud tento záznam není spojený s uživatelem tak to oznámit aktualně hledajícímu uživately
                if(!Showing.IsLinkedToUSer)
                {
                    DisplayAlert("", "Záznam " + LRDB.GetName(Showing.Id).Name + " Není ještě spojený s uživatelem. Prosím, přiveďte tohoto tvora, kterýmu patří tento záznam, na tuto aplikaci. ", "OK");
                }
                // Najít hodnocení na tento záznam od ostatních záznamů
                foreach (Classes.Record Rating in Selected)
                {
                    if (Showing != Rating)
                    {
                        // pokud je tento hodnotící záznam připojený k uživately, tak přidej jeho hodnocení k tomuto záznamu který je honocen
                        if(Rating.IsLinkedToUSer)
                        {
                            Button button = new Button();
                            Classes.Ratinging rating = LRDB.GetUsersRatingOfRecord(Showing.Id, Rating.LinkedUserID);
                            // pokud ještě není hodnocen tímto uživatelem, tak vytvoř bílí tlačítko
                            if (rating == null)
                            {
                                button = new Button
                                {
                                    BorderColor = Color.Black,
                                    WidthRequest = 40,
                                    HeightRequest = 40,
                                    BorderWidth = 3,
                                    ClassId = Rating.Id.ToString(),
                                    BackgroundColor = Color.FromRgb(255, 255, 255)
                                };
                            }
                            else
                            {
                                button = new Button();
                                // pokud je hodnocení pozitvní ubírej ze žlutého tlačítka červenou barvu
                                if (rating.Rate > 0)
                                {
                                    int RRGB = 255;
                                    int RGBT = int.Parse((Math.Round((rating.Rate * 2.55), 0, MidpointRounding.ToEven)).ToString());
                                    RRGB = RRGB - RGBT;
                                    button = new Button
                                    {
                                        BorderColor = Color.Black,
                                        WidthRequest = 40,
                                        HeightRequest = 40,
                                        BorderWidth = 3,
                                        ClassId = Rating.Id.ToString(),
                                        BackgroundColor = Color.FromRgb(RRGB, 255, 0)
                                    };
                                }
                                // pokud je hodnocení negativní ubírej ze žlutého tlačítka zelenou barvu
                                else if (rating.Rate < 0)
                                {
                                    int GRGB = 255;
                                    int RGBT = int.Parse((Math.Round((rating.Rate * 2.55), 0, MidpointRounding.ToEven)).ToString());
                                    GRGB = GRGB + RGBT;
                                    button = new Button
                                    {
                                        BorderColor = Color.Black,
                                        WidthRequest = 40,
                                        HeightRequest = 40,
                                        BorderWidth = 3,
                                        ClassId = Rating.Id.ToString(),
                                        BackgroundColor = Color.FromRgb(255, GRGB, 0)
                                    };
                                }
                                // pokud je hodnocení neutrální necháme tlačítko žluté
                                else
                                {
                                    button = new Button
                                    {
                                        BorderColor = Color.Black,
                                        WidthRequest = 40,
                                        HeightRequest = 40,
                                        BorderWidth = 3,
                                        ClassId = Rating.Id.ToString(),
                                        BackgroundColor = Color.FromRgb(255, 255, 0)
                                    };
                                }
                                
                            }
                            // značení tlačítek
                            bool WasSelected = false;
                            // pokud je tlačítko vytvořený z honocení aktuálně hledajícího uživatele
                            if (Rating.Id == LRDB.GetUsersRecord(LUDB.WhoLogged().Id).Id)
                            {
                                // Tak do talčítka přidat text "Ty"
                                button.Text = "Ty";
                                button.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
                                button.FontAttributes = FontAttributes.Bold;
                                WasSelected = true;
                            }
                            // pokud tlačítko není vytvoření z hodnocení z původních zadaných záznamů nebo právš hledajícího uživatele
                            foreach (Classes.Namies Firstly in selected )
                            {
                                if(Firstly.RecordID == Rating.Id)
                                {
                                    WasSelected = true;
                                }
                            }
                            if(!WasSelected)
                            {
                                // tak vytvoř z tlačítka kruh
                                button.CornerRadius = 20;
                            }
                            button.Clicked += Hide_Clicked;
                            Seed.Children.Add(button);
                            
                        }
                    }
                    


                }

                Shell.Content = Seed;
                Crust.Children.Add(Shell);
                Crust.Children.Add(Hide);
                Crust.Children.Add(Unhide);
                Peel.Content = Crust;
                Names_Stack.Children.Add(Peel);
            } 
            // https://stackoverflow.com/questions/46012446/change-xamarin-forms-button-color
            //https://www.geeksforgeeks.org/c-sharp-math-round-method-set-2/
        }

        private void Hide_Clicked(object sender, EventArgs e)
        {
            //když se zmáčkne tlačítko s korespondujícím záznamem nebo jeho hodnocení jenýho záznamu 
            string IDToHide = (sender as Button).ClassId;
            Hidden.Add(int.Parse(IDToHide));
            foreach (Frame Peel in Names_Stack.Children)
            {
                // tak se jeho záznam "vypne"
                StackLayout Crust = (Peel.Content as StackLayout);
                if ((Crust.Children[1] as Button).ClassId == IDToHide)
                {
                    foreach (var button in ((Crust.Children[0] as ScrollView).Content as StackLayout).Children)
                    {
                        if (button is Button)
                        {
                            (button as Button).IsVisible = false;
                        }
                        if (button is Label)
                        {
                            (button as Label).TextColor = Color.Gray;
                        }
                    }
                    (Crust.Children[1] as Button).IsVisible = false;
                    (Crust.Children[2] as Button).IsVisible = true;
                }
                else
                {
                    // a jeho hodnocení ostatních skryje
                    foreach (var button in ((Crust.Children[0] as ScrollView).Content as StackLayout).Children)
                    {
                        if (button is Button)
                        {
                            if ((button as Button).ClassId == IDToHide)
                            {
                                (button as Button).IsVisible = false;
                            }
                        }
                    }
                }
            }
        }
        private void Unhide_Clicked(object sender, EventArgs e)
        {
            //když se zmáčkne tlačítko s korespondujícím záznamem, který ´je "vypnutý"
            string IDToHide = (sender as Button).ClassId;
            Hidden.Remove(int.Parse(IDToHide));
            foreach (Frame Peel in Names_Stack.Children)
            {
                // tak se jeho záznam "Zapne"
                StackLayout Crust = (Peel.Content as StackLayout);
                if ((Crust.Children[1] as Button).ClassId == IDToHide)
                {
                    foreach (var button in ((Crust.Children[0] as ScrollView).Content as StackLayout).Children)
                    {
                        if (button is Button)
                        {
                            bool isHidden = false;
                            foreach (int Hid in Hidden)
                            {
                                if (Hid == int.Parse((button as Button).ClassId))
                                {
                                    isHidden = true;                                  
                                }
                            }
                            if (!isHidden)
                            {
                                (button as Button).IsVisible = true;

                        }
                    }
                        if (button is Label)
                        {
                            (button as Label).TextColor = Color.Black;
                        }
                    }
                    (Crust.Children[1] as Button).IsVisible = true;
                    (Crust.Children[2] as Button).IsVisible = false;
                }
                else
                {
                    // Jeho hodnocení ostních se objeví
                    foreach (var button in ((Crust.Children[0] as ScrollView).Content as StackLayout).Children)
                    {
                        if (button is Button)
                        {
                            if((Crust.Children[1] as Button).IsVisible == true)
                            {
                                if ((button as Button).ClassId == IDToHide)                                
                                {
                                        (button as Button).IsVisible = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}