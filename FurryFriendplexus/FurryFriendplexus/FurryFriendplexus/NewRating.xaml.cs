using Syncfusion.SfAutoComplete.XForms;
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
        public bool Himself = false;
        public List<Classes.Namies> Names = new List<Classes.Namies>();
        public Classes.Record Selected = null;
        Classes.Ratinging AlredyRated = null;

        public NewRating(bool himself)
        {
            List<string> AutoCompleteItems = new List<string>();
            Names = LRDB.GelAllNames();            
            foreach (Classes.Namies Name in Names)
            {
                AutoCompleteItems.Add(Name.Name);
            }
            InitializeComponent();
            Nanana.AutoCompleteSource = AutoCompleteItems;
            Himself = himself;
            if(himself)
            {
                RatingSlider.IsVisible = false;
                NumberDisplay.IsVisible = false;
                PercentLabel.IsVisible = false;
            }
        }

        // Určení posuvníku použe na cvelá čísla a aktualizování jeho číselníku
        void OnSliderValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1.0);
            RatingSlider.Value = newStep * 1.0;

            NumberDisplay.Text = RatingSlider.Value.ToString() + "%";
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
                if (NameNumber != 0)
                { 
                    StackLayout Namer = Nameris as StackLayout;                    
                    (Namer.Children[1] as Button).ClassId = NameNumber.ToString();
                }
            }
        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            if (Selected == null)
            {
                if ((Names_Stack.Children[0] as SfAutoComplete).Text != null && RaceInput.Text != null )
                { 
                    Classes.Record Newbie = new Classes.Record
                    {
                        Race = RaceInput.Text,
                        IsLinkedToUSer = false
                    };
                    Classes.Ratinging NewRating = new Classes.Ratinging
                    {
                        Rate = int.Parse(RatingSlider.Value.ToString()),
                        RaterUserID = LUDB.WhoLogged().Id
                    };
                    List<Classes.Namies> NewNamies = new List<Classes.Namies>();
                    int NameNumber = 0;
                    foreach (var InputNames in Names_Stack.Children)
                    {
                        NameNumber++;
                        if (NameNumber == 1)
                        {
                            NewNamies.Add(new Classes.Namies { Name = (InputNames as SfAutoComplete).Text });
                        }
                        else
                        {
                            if (((InputNames as StackLayout).Children[0] as Entry).Text != null)
                            { 
                                NewNamies.Add(new Classes.Namies { Name = ((InputNames as StackLayout).Children[0] as Entry).Text });
                            }   
                            else
                            {
                                DisplayAlert("", "Přídavné políčko jména nebylo vyplněno.", "OK");
                            }
                        }
                    }
                    if (Himself)
                    {
                        Newbie.LinkedUserID = LUDB.WhoLogged().Id;
                        Newbie.IsLinkedToUSer = true;
                        LRDB.SaveNewUsersRecord(Newbie, NewNamies);
                    }
                    else
                    {

                        LRDB.SaveNewRecord(Newbie, NewNamies, NewRating);
                    }
                }
                else
                {
                    DisplayAlert("", "Jméno a rasa musejí být vyplněný.", "OK");
                }
            }
            else
            {
                if (Himself)
                {
                    Selected.LinkedUserID = LUDB.WhoLogged().Id;
                    Selected.IsLinkedToUSer = true;
                    LRDB.RecordHaveRegistered(Selected);
                }
                else
                { 
                    if (AlredyRated == null)
                    {
                        Classes.Ratinging newRating = new Classes.Ratinging
                        {
                            Rate = int.Parse(RatingSlider.Value.ToString()),
                            RaterUserID = LUDB.WhoLogged().Id,
                            RecordID = Selected.Id
                        };
                        LRDB.SaveRating(newRating);
                    }
                    else
                    {
                        AlredyRated.Rate = int.Parse(RatingSlider.Value.ToString());
                        LRDB.UpdateRating(AlredyRated);
                    }
                }
                foreach (var InputNames in Names_Stack.Children)
                {
                    if (InputNames is StackLayout)
                    { 
                        if ((InputNames as StackLayout).Children[0] is Entry)
                        {
                            Classes.Namies newName = new Classes.Namies { Name = ((InputNames as StackLayout).Children[0] as Entry).Text, RecordID = Selected.Id };
                            LRDB.SaveNewName(newName);
                        }
                    }
                }

            }
            if(Himself)
            {
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
                Navigation.PopModalAsync();
            }
            else
            { 
                Navigation.PopAsync();
            }

        }

        private void NewRating_Appearing(object sender, EventArgs e)
        {
            if ( LUDB.WhoLogged().Id == -1)
            {
                Navigation.PopAsync();
            }
        }
        private void Nanana_Unfocused(object sender, FocusEventArgs e)
        {
            foreach (Classes.Namies name in Names)
            {
                if (Nanana.Text == name.Name && (!Himself || ( Himself && !LRDB.FindRecord(name.RecordID).IsLinkedToUSer)) && LRDB.FindRecord(name.RecordID).Id != LUDB.WhoLogged().Id)
                {
                    Selected = LRDB.FindRecord(name.RecordID);
                    foreach ( var Children in Names_Stack.Children)
                    {
                        if( Children is SfAutoComplete)
                        {
                            Children.IsVisible = false;
                        }
                        else if ( Children is Entry)
                        {
                            Names_Stack.Children.Remove(Children);
                        }
                    }
                    RaceInput.IsVisible = false;
                    GivenRace.IsVisible = true;
                    int NameRNumber = 0;
                    foreach (Classes.Namies nameR in Names)
                    {
                        if ( nameR.RecordID == Selected.Id)
                        {                            
                            if (NameRNumber == 0)
                            {
                                StackLayout newStackLayout = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal,
                                    VerticalOptions = LayoutOptions.Start,
                                    HorizontalOptions = LayoutOptions.Fill

                                };
                                Label NewLabel = new Label
                                {
                                    Text = nameR.Name,
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                    VerticalOptions = LayoutOptions.StartAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand
                                };
                                Button newButton = new Button
                                {
                                    Text = "X",
                                    TextColor = Color.Red,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                    HorizontalOptions = LayoutOptions.End,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                };
                                newButton.Clicked += Nanana_Defocused;
                                newStackLayout.Children.Add(NewLabel);
                                newStackLayout.Children.Add(newButton);
                                Names_Stack.Children.Add(newStackLayout);
                            }
                            else
                            {
                                Label NewLabel = new Label
                                {
                                    Text = nameR.Name,
                                    VerticalOptions = LayoutOptions.Start,
                                    HorizontalOptions = LayoutOptions.Fill,
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                                };
                                Names_Stack.Children.Add(NewLabel);
                            }
                            NameRNumber++;
                        }
                }
                    GivenRace.Text = Selected.Race;
                     AlredyRated = LRDB.GetUsersRatingOfRecord(Selected.Id, LUDB.WhoLogged().Id);
                    if (AlredyRated != null)
                    {
                        RatingSlider.Value = AlredyRated.Rate;
                    }
                }
                else if (Nanana.Text == name.Name && Himself && LRDB.FindRecord(name.RecordID).IsLinkedToUSer)
                {                    
                    DisplayAlert("", "Jako tento záznam už je někdo zaregistrovaný.", "OK");
                    Nanana.Text = "";
                }
                else if (Nanana.Text == name.Name && LRDB.FindRecord(name.RecordID).Id == LUDB.WhoLogged().Id)
                {
                    DisplayAlert("", "Nemůžete hodnotit sám sebe", "OK");
                    Nanana.Text = "";
                }

            }
        }
        private void Nanana_Defocused(object sender, EventArgs e)
        {
            RaceInput.IsVisible = true;
            GivenRace.IsVisible = false;
            int countOfRecord = 0;
            List<int> countersOfRecords = new List<int>();
            foreach (var Children in Names_Stack.Children)
            {
                if (Children is SfAutoComplete)
                {
                    Children.IsVisible = true;
                }
                else 
                {
                    countersOfRecords.Add(countOfRecord);
                }
                countOfRecord++;
            }
            countersOfRecords.Reverse();
            foreach (int recordID in countersOfRecords)
            {

                Names_Stack.Children.Remove(Names_Stack.Children[recordID]); 
            };
            Selected = null;
            AlredyRated = null;
        }
    }
}