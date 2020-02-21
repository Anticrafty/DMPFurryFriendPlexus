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
        // objekt databáze pro uživatele
        public LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        // Objekt databáze pro záznamy a jejich jména a hodnocení
        public LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();
        // Jestli ej za cíl hodnotit někoho nebo jen vytvořit záznam aktuálního uživatele
        public bool Himself = false;
        //  seznam jmen pro našeptávače
        public List<Classes.Namies> Names = new List<Classes.Namies>();
        // objekt pro záznam poku se chce jen upravovat
        public Classes.Record Selected = null;
        // objekt pro honocení záznamu od tohoto uživatele
        Classes.Ratinging AlredyRated = null;

        public NewRating(bool himself)
        {
            // naládování našeptávače
            List<string> AutoCompleteItems = new List<string>();
            Names = LRDB.GelAllNames();            
            foreach (Classes.Namies Name in Names)
            {
                AutoCompleteItems.Add(Name.Name);
            }
            InitializeComponent();
            Nanana.AutoCompleteSource = AutoCompleteItems;
            Himself = himself;
            // pokud jen se pojí ke svému záznamu, tak se nemůže hodnotit 
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
        // funkce tlačítka na zaslání zánamu a jeho jmen a hodnocení
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            // pokud je to nový záznam
            if (Selected == null)
            {
                // pokud je zadáno hlavní jméno a živočišný druh
                if ((Names_Stack.Children[0] as SfAutoComplete).Text != null && RaceInput.Text != null )
                { 
                    //Tak vložit svých do objektů
                    // Nový záznam
                    Classes.Record Newbie = new Classes.Record
                    {
                        Race = RaceInput.Text,
                        IsLinkedToUSer = false
                    };
                    // Nový hodncení
                    Classes.Ratinging NewRating = new Classes.Ratinging
                    {
                        Rate = int.Parse(RatingSlider.Value.ToString()),
                        RaterUserID = LUDB.WhoLogged().Id
                    };
                    // A všecna jeho jména které jsou zadána
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
                                // pokud políčko jména, které není hlavní, je prázné, tak se nepřidá do objektu a upozorní se atuální uživatel
                                DisplayAlert("", "Přídavné políčko jména nebylo vyplněno.", "OK");
                            }
                        }
                    }
                    // pokud hodnotí sebe
                    if (Himself)
                    {
                        // tak se záznam spojí s jeho uživatelským záznamem
                        Newbie.LinkedUserID = LUDB.WhoLogged().Id;
                        Newbie.IsLinkedToUSer = true;
                        // a celí se to pošle
                        LRDB.SaveNewUsersRecord(Newbie, NewNamies);
                    }
                    else
                    {
                        // pokud nehodnotí sebe, tak se to už pošle
                        LRDB.SaveNewRecord(Newbie, NewNamies, NewRating);
                    }
                }
                else
                {
                    // pokud není hlavní jméno nebo živočišný druh zadán tak e upozorní aktualní uživatel
                    DisplayAlert("", "Jméno a rasa musejí být vyplněný.", "OK");
                }
            }
            // pokud už záznam exituje
            else
            {
                // pokud zaznamenává sebe
                if (Himself)
                {
                    // tak se existujícímu záznamu připíše aktuální uživatel
                    Selected.LinkedUserID = LUDB.WhoLogged().Id;
                    Selected.IsLinkedToUSer = true;
                    LRDB.RecordHaveRegistered(Selected);
                }
                // pokud upravuje záznam někoho jinýho
                else
                { 
                   // tak pokud nebyl záznam ještě hodnocen aktuáním uživatelem
                    if (AlredyRated == null)
                    {
                        // tak se zapíše nové hodnocení tohoto záznamu od aktuálního uživatele
                        Classes.Ratinging newRating = new Classes.Ratinging
                        {
                            Rate = int.Parse(RatingSlider.Value.ToString()),
                            RaterUserID = LUDB.WhoLogged().Id,
                            RecordID = Selected.Id
                        };
                        LRDB.SaveRating(newRating);
                    }
                    // pokud už byl hodnocen aktuáním uživatelem
                    else
                    {
                        // tak se jen změní 
                        AlredyRated.Rate = int.Parse(RatingSlider.Value.ToString());
                        LRDB.UpdateRating(AlredyRated);
                    }
                }
                // zkontoluje se jestli nebylo zadáno nevé jméno, pokud jo, tak se nevé jména zabalí do objektu a zapaíšou
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
            // poté se stránka uzavře
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
        // pokud uživatel není přihlášený, tak mu nedovolíme jít na tuto stránku
        private void NewRating_Appearing(object sender, EventArgs e)
        {
            if ( LUDB.WhoLogged().Id == -1)
            {
                Navigation.PopAsync();
            }
        }
        // pokud uživatel zapsal nové jméno
        private void Nanana_Unfocused(object sender, FocusEventArgs e)
        {
            //, tak zkontrolujeme, jestli jméno nepatří již nějakému záznamu
            foreach (Classes.Namies name in Names)
            {
                // pokud patří nějakému záznamu a pokud hodnozí sebe atento záznam nikomu nepatří a jestli záznam nepatří jemu samontnému.
                if (Nanana.Text == name.Name && (!Himself || ( Himself && !LRDB.FindRecord(name.RecordID).IsLinkedToUSer)) && LRDB.FindRecord(name.RecordID).Id != LUDB.WhoLogged().Id)
                {
                    // tak předzadej všechny prvky a shovej místa pro doplnění
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
                // pokud má zaznamenat svůj záznama někdo už je za tento záznam zaznamenán
                else if (Nanana.Text == name.Name && Himself && LRDB.FindRecord(name.RecordID).IsLinkedToUSer)
                {               
                    // tak se upozorní uživatel a zruší se výběr
                    DisplayAlert("", "Jako tento záznam už je někdo zaregistrovaný.", "OK");
                    Nanana.Text = "";
                }
                // pokud uživatel vybere na hodnocení sebe
                else if (Nanana.Text == name.Name && LRDB.FindRecord(name.RecordID).Id == LUDB.WhoLogged().Id)
                {
                    // tak se upozorní uživatel a zruší se výběr
                    DisplayAlert("", "Nemůžete hodnotit sám sebe", "OK");
                    Nanana.Text = "";
                }

            }
        }
        // funkce pro zrušení aktuálního určenýho záznamu
        private void Nanana_Defocused(object sender, EventArgs e)
        {
            // tak se vymažou všechny předvyplněné údaje 
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
            // a nechají se objevit zadávací místa
            Selected = null;
            AlredyRated = null;
        }
    }
}