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
                Text = Nanana.Text + Names_Stack.Children.Count().ToString(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 10, 10, 10)
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
                if (NameNumber != 1)
                {
                    StackLayout Namer = Nameris as StackLayout;
                    (Namer.Children[1] as Button).ClassId = NameNumber.ToString();
                }
            }
        }
    }
}