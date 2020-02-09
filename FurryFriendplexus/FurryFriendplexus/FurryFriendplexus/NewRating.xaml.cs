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
    public partial class NewRating : ContentPage
    {
        public LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();

        public NewRating()
        {
            InitializeComponent();
        }

        // Určení posuvníku použe na cvelá čísla a aktualizování jeho číselníku
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1.0);
            RatingSlider.Value = newStep * 1.0;

            Ciselnik.Text = RatingSlider.Value.ToString() + "%";
        }

        // Přidávání Inputu pro Jména
        private void Plus_Clicked(object sender, EventArgs e)
        {
            // Obal záznamu
            StackLayout Stacky = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            // Jmeno Záznamu
            Entry New_Name = new Entry
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                //https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/text/fonts
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                // https://stackoverflow.com/questions/24034204/xamarin-forms-margins
            };
            // Tlačítko na Smazání záznamu
            Button Nope = new Button
            {
                Text = "X",
                TextColor = Color.Red,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ClassId = Names_Stack.Children.Count().ToString()
                //https://stackoverflow.com/questions/42858449/get-control-name-in-button-event-handler-method-xamarin-forms/42859064
            };
            // Přidání Spojení s funkcí pro mazání záznamu
            Nope.Clicked += X_Clicked;
            //https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/button
            // Dávání záznamu do Stránky
            Stacky.Children.Add(New_Name);
            Stacky.Children.Add(Nope);
            Names_Stack.Children.Add(Stacky);
        }

        // Mazání záznamů
        private void X_Clicked(object sender, EventArgs e)
        {
            // Zjišťování kdo volal
            Button Caller = sender as Button;
            int NameNumber = int.Parse(Caller.ClassId);
            // Mazání zavolaného záznamu
            Names_Stack.Children.Remove(Names_Stack.Children[NameNumber]);
            // Přečislovávání záznamů
            NameNumber = -1;
            foreach ( var Nameris in Names_Stack.Children ) 
            {
                NameNumber++;
                if (NameNumber != 1)
                { 
                    StackLayout Namer = Nameris as StackLayout;                    
                    (Namer.Children[1] as Button).ClassId = NameNumber.ToString();
                }
            }
        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            Classes.Record Newbie = new Classes.Record
            {
                Race = RaceInput.Text
            };
            Classes.Ratinging NewRating = new Classes.Ratinging
            {
                Rate = int.Parse(RatingSlider.Value.ToString()),
                RaterUserID = LUDB.WhoLogged().Id
                
            };
            List<Classes.Namies> NewNamies = new List<Classes.Namies>();
            int NameNumber = 0;
            foreach ( var InputNames in Names_Stack.Children)
            {
                NameNumber++;
                if(NameNumber == 1)
                {
                }
                else if(NameNumber == 2)
                {
                    NewNamies.Add(new Classes.Namies { Name = (InputNames as Entry).Text });
                } 
                else
                {
                    NewNamies.Add(new Classes.Namies { Name = ((InputNames as StackLayout).Children[0] as Entry).Text });
                }
            }
            LRDB.SaveNewRecord(Newbie, NewNamies, NewRating);
            Navigation.PopAsync();
        }

        private void NewRating_Appearing(object sender, EventArgs e)
        {
            if ( LUDB.WhoLogged().Id == -1)
            {
                Navigation.PopAsync();
            }
        }
    }
}