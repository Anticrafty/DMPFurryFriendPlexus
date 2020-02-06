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

        private void Add_Clicked(object sender, EventArgs e)
        {
            StackLayout Stacky = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            Label New_Name = new Label
            {
                Text = Nanana.Text,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };
            Button Nope = new Button
            {
                Text = "X",
                TextColor = Color.Red,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                ClassId = Names_Stack.Children.Count().ToString()
            };
            Nope.Clicked += X_Clicked;
            Stacky.Children.Add(New_Name);
            Stacky.Children.Add(Nope);
            Names_Stack.Children.Add(Stacky);
            NameCounter.Text = "Počet Zadaných: " + Names_Stack.Children.Count().ToString();
        }

        private void X_Clicked(object sender, EventArgs e)
        {
            Button Caller = sender as Button;
            int NameNumber = int.Parse(Caller.ClassId);
            Names_Stack.Children.Remove(Names_Stack.Children[NameNumber]);
            NameNumber = -1;
            foreach (var Nameris in Names_Stack.Children)
            {
                NameNumber++;
                StackLayout Namer = Nameris as StackLayout;
                (Namer.Children[1] as Button).ClassId = NameNumber.ToString();
            }
            NameCounter.Text = "Počet Zadaných: " + Names_Stack.Children.Count().ToString();
        }

        private void Friends_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox CH = sender as CheckBox;

            if (CH.ClassId == "PCH" & CH.IsChecked == true)
            {
                DPCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "DPCH" & CH.IsChecked == true)
            {
                PCH.IsChecked = false;
                BPCH.IsChecked = false;
            }
            else if (CH.ClassId == "BPCH" & CH.IsChecked == true)
            {
                PCH.IsChecked = false;
                DPCH.IsChecked = false;
            }
        }
    }
}