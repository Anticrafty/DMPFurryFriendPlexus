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
        // objekt databáze pro uživatele
        LocalDB.LocalUserDB LUDB = new LocalDB.LocalUserDB();
        public Registration()
        {
            InitializeComponent();
        }
        // funkce tlačítka na odeslání pokusu o registraci
        private void Confirm_Clicked(object sender, EventArgs e)
        {
            bool AllOk = true;
            // zkontrolovat, jestli zadané přihlašovací, jméno, již není zadáno jiným uživatelem
            if (LUDB.TryUsername(UsernameE.Text))
            {
                // pokud je tak, smažeme z políčka a oznámíme uživateli
                UsernameE.Text = "";
                DisplayAlert("", "Toto jméno už je zabrané", "OK");
                // Source: https://docs.microsoft.com/cs-cz/xamarin/xamarin-forms/user-interface/pop-ups
                AllOk = false;
            } 
            // Zkontolujeme, jestli se nějaké uživatelské jméno zadalo
            else if (UsernameE.Text == null )
            {
                // pokud ne. Upozorníme
                DisplayAlert("", "Musíte mít nějaké Uživatelské jméno", "OK");
                AllOk = false;
            }
           // pokud je heslo prázně nebo moc krádké tak upozorníme
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
             // pokud se kontrola hesla neshoduje s heslem
            else if (PasswordE.Text != PasswordTest.Text)
            {
                // tak záznamy z kolonek vymažeme a upozorníme
                PasswordE.Text = "";
                PasswordTest.Text = "";
                DisplayAlert("", "Hesla se nehodují", "OK");
                AllOk = false;
            }
             // pokiud je vše v pořádku
            if (AllOk)
            {
                // zahešujeme tímto heslo
                SHA256 sha256Hash = SHA256.Create();
                // Nového uživatele zabalíme a zapíšeme
                Classes.Users user = new Classes.Users { Nickname = UsernameE.Text, Password = GetHash(sha256Hash, PasswordE.Text), IsLogged = true};
                LUDB.RegisterHim(user);
                // uživatele o zaregistrování upozorníme
                DisplayAlert("", "Byl jste zaregistrován", "OK");
                // a pošleme ho se spojit se záznamem
                Navigation.PushModalAsync(new NewRating(true));
            }
        }
        // funkce tlačítka se vrátit na stránku s přihlášením
        private void Login_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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