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
    public partial class Login : ContentPage
    {
        // objekt databáze pro uživatele
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public Login()
        {
            InitializeComponent();
            // pokud je někdo přihlášený, tak se tato stránka závře
            if (LUDB.LookAtLocalLogin())
            {
                Navigation.PopModalAsync();
            }

        }
        // funkce tlačítka na odeslání pokusu o přihášení
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            // standard na hashování hesla
            SHA256 sha256Hash = SHA256.Create();
            // vkládání údajů do objektu a hashování hesla
            Classes.Users user = new Classes.Users { Nickname = UsernameE.Text, Password = GetHash(sha256Hash, PasswordE.Text) };
            // poslat objekt do pokusu o přihlášení
            if(LUDB.LoginHim(user))
            {
                // pokud pokus proběhne uspěšně, stránka se zavře 
                Navigation.PopModalAsync();
            }
            else
            {
                // pokud pokus proběhne neúspěšně, tka to oznam uživateli
                DisplayAlert("", "Jmeno nebo heslo jsou neplatné", "OK");
                PasswordE.Text = "";
            }
            
        }
        // funkce tlačítka na dostání se na stránku s registrací
        private void Registr_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Registration());
        }

        // Funkce na hashování hesla vzatá ze stránky napsané pod funkcí, která má kód sama zakomentovaný
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