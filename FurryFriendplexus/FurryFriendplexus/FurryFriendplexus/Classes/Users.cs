using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FurryFriendplexus.Classes
{
    //Objekt vytvořený pro práci s Uživately
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        // Identifikační číslo pro tohoto uživatele
        public int Id { get; set; }
        [Indexed]
        // Přihlašovací jméno pro uživatele
        public string Nickname { get; set; }
        // Heslo pro uživatele
        public string Password { get; set; }
        // Kontrola jestli je uživatel přihlášen
        public bool IsLogged { get; set; }
}
}
