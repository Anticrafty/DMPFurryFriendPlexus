﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FurryFriendplexus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGroup : ContentPage
    {
        LocalDB.LocalRatingDB LRDB = new LocalDB.LocalRatingDB();
        List<Classes.Namies> Names = new List<Classes.Namies>();
        public NewGroup()
        {
            InitializeComponent();
            Actualize_NameList();
        }
        public void Actualize_NameList()
        {

            List<string> AutoCompleteItems = new List<string>();
            Names = LRDB.GelAllNames();
            foreach (Classes.Namies Name in Names)
            {
                AutoCompleteItems.Add(Name.Name);
            }
             Nanana.AutoCompleteSource = AutoCompleteItems;
            // sources: https://forums.xamarin.com/discussion/140933/autocomplete-entry-in-xamarin-forms
            // https://help.syncfusion.com/xamarin/autocomplete/getting-started?cs-save-lang=1&cs-lang=csharp
        }
         // Přidávání záznamů z Inputu do seznamu
        private void Add_Clicked(object sender, EventArgs e)
        {
            bool OkToGo = true;

            bool NameExists = false;
            foreach (Classes.Namies name in Names)
            {
                if (name.Name == Nanana.Text)
                {
                    NameExists = true;
                }
            }
            if (!NameExists)
            {
                OkToGo = false;
                DisplayAlert("", "Tento záznam neexistuje.", "OK");
            }

            bool AlredyInTheList = false;
            int IDOfSelected = -1;
            foreach (Classes.Namies name in Names)
            {
                if (name.Name == Nanana.Text)
                {
                    IDOfSelected = name.RecordID;
                }
            }
            foreach (StackLayout StackInList in Names_Stack.Children)
            {
                foreach (Classes.Namies name in Names)
                {
                    if (name.Name == (StackInList.Children[0] as Label).Text)
                    {
                        if(name.RecordID == IDOfSelected)
                        {
                            OkToGo = false;
                            DisplayAlert("", "Tento záznam už je zadaný v seznamu.", "OK");
                        }
                    }
                }
            }

            if(OkToGo)
            { 
                // Obal záznamu
                StackLayout Stacky = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };
                // Jmeno Záznamu
                Label New_Name = new Label
                {
                    Text = Nanana.Text,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };
                Nanana.Text = "";
                // Tlačítko na Smazání záznamu
                Button Nope = new Button
                {
                    Text = "X",
                    TextColor = Color.Red,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    ClassId = Names_Stack.Children.Count().ToString()
                };
                // Přidání Spojení s funkcí pro mazání záznamu
                Nope.Clicked += X_Clicked;
                // Přidávání názvu a tlačítka do Obalu
                Stacky.Children.Add(New_Name);
                Stacky.Children.Add(Nope);
                // Dávání záznamu do Seznamu
                Names_Stack.Children.Add(Stacky);
                // Aktualizování počítadla
                NameCounter.Text = "Počet Zadaných: " + Names_Stack.Children.Count().ToString();
                
            }
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
            foreach (var Nameris in Names_Stack.Children)
            {
                NameNumber++;
                StackLayout Namer = Nameris as StackLayout;
                (Namer.Children[1] as Button).ClassId = NameNumber.ToString();
            }
            // Aktualizovaní Počítadla
            NameCounter.Text = "Počet Zadaných: " + Names_Stack.Children.Count().ToString();
        }
        // Výběr pouze Jedný úrovně Přátelství 
        private void Friends_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox CH = sender as CheckBox;

            // pokud jedno se změnilo 
            if (CH.ClassId == "PCH" & PCH.IsChecked == true)
            {
                // Kromě toho kde se to změnilo, se to za odškrtne
                DPCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "DPCH" & DPCH.IsChecked == true)
            {
                DPCH.IsChecked = true;
                PCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "BPCH" & BPCH.IsChecked == true)
            {
                BPCH.IsChecked = true;
                PCH.IsChecked = false;
                DPCH.IsChecked = false;
            }
        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            if(Names_Stack.Children.Count() != 0)
            {
                if (DPCH.IsChecked == true || PCH.IsChecked == true || BPCH.IsChecked == true)
                {
                    List<Classes.Namies> SelectedNames = new List<Classes.Namies>();
                    int LevelOfSearch = 0;
                    foreach(StackLayout StackInList in Names_Stack.Children)
                    {
                        foreach( Classes.Namies names in Names)
                        {
                            if ( (StackInList.Children[0] as Label).Text == names.Name)
                            {
                                SelectedNames.Add(names);
                            }
                        }
                    }
                    if (PCH.IsChecked)
                    {
                        LevelOfSearch = 1;
                    }
                    else if (DPCH.IsChecked)
                    {
                        LevelOfSearch = 2;
                    } 
                    else if (BPCH.IsChecked)
                    {
                        LevelOfSearch = 3;
                    }
                    Navigation.PushAsync(new Comparison(SelectedNames, LevelOfSearch));

                }
                else
                {
                    DisplayAlert("", "Musí se zaškrtnout jakou úroveň přátelství se má hledat.", "OK");
                }
            }
            else
            {
                DisplayAlert("", "V seznamu musí být ňějaký záznam.", "OK");
            }
        }
    }
}