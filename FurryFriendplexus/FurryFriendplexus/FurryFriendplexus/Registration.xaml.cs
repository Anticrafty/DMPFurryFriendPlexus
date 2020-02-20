using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FurryFriendplexus
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {

        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public Registration()
        {
            InitializeComponent();
        }
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            bool AllOk = true;
            if (LUDB.TryUsername(UsernameE.Text))
            {
                UsernameE.Text = "";
                DisplayAlert("", "Toto jméno už je zabrané", "OK");
                // Source: https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/pop-ups
                AllOk = false;
            } else if (UsernameE.Text == null )
            {
                DisplayAlert("", "Musíte mít nějaké Uživatelské jméno", "OK");
                AllOk = false;
            }
           
             if (PasswordE.Text == null )
            {
                DisplayAlert("", "Heslo je moc krátké", "OK");
                AllOk = false;
            }  
            else if (PasswordE.Text.Length < 8)
            {
                DisplayAlert("", "Heslo je moc krátké", "OK");
                AllOk = false;
            }
            else if (PasswordE.Text != PasswordTest.Text)
            {
                PasswordE.Text = "";
                PasswordTest.Text = "";
                DisplayAlert("", "Hesla se nehodují", "OK");
                AllOk = false;
            }
            if (AllOk)
            {
                SHA256 sha256Hash = SHA256.Create();
                Classes.Users user = new Classes.Users { Nickname = UsernameE.Text, Password = GetHash(sha256Hash, PasswordE.Text), IsLogged = true};
                LUDB.RegisterHim(user);

                DisplayAlert("", "Byl jste zaregistrován", "OK");
                Navigation.PushModalAsync(new NewRating(true));
            }
        }
        private void Login_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        //https://docs.microsoft.com/cs-cz/dotnet/api/system.security.cryptography.hashalgorithm.computehash?view=netframework-4.8
    }
}