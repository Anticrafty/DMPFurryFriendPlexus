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


        public NewRating()
        {
            InitializeComponent();
        }

        // Snapping slider on integers
        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1.0);
            RatingSlider.Value = newStep * 1.0;

            Ciselnik.Text = RatingSlider.Value.ToString() + "%";
        }

        private void Plus_Clicked(object sender, EventArgs e)
        {
            StackLayout Stacky = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            Label New_Name = new Label
            {
                Text = "Anti Crafty" + Names_Stack.Children.Count().ToString(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                //https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/text/fonts
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 10, 10, 10)
                // https://stackoverflow.com/questions/24034204/xamarin-forms-margins
            };
            Button Nope = new Button
            {
                Text = "X",
                TextColor = Color.Red,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                ClassId = Names_Stack.Children.Count().ToString()
                //https://stackoverflow.com/questions/42858449/get-control-name-in-button-event-handler-method-xamarin-forms/42859064
            };
            Nope.Clicked += X_Clicked;
            //https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/button
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
    }
}