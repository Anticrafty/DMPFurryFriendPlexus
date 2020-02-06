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
    public partial class NewGroup : ContentPage
    {
        public NewGroup()
        {
            InitializeComponent();
        }

        // Přidávání záznamů z Inputu do seznamu
        private void Add_Clicked(object sender, EventArgs e)
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
            if (CH.ClassId == "PCH")
            {
                // Tak tam kde se to změnilo se to zaškrtne a ostatní se za odškrtne
                CH.IsChecked = true;
                DPCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "DPCH")
            {
                CH.IsChecked = true;
                PCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "BPCH")
            {
                CH.IsChecked = true;
                PCH.IsChecked = false;
                DPCH.IsChecked = false;
            }
        }
    }
}