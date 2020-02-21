using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FurryFriendplexus.Classes;

namespace FurryFriendplexus.LocalDB
{
    // Objekt pro práci s částí databáze určený pro Uživatele 
    public class LocalUserDB
    {
        //https://github.com/praeclarum/sqlite-net/blob/master/README.md
        ////cesta pro lokální databázy 
        private string databasePath;
        // Samotný objek databáze
        private SQLiteConnection db;
        public LocalUserDB()
        {
            // Zjištění cesty pro databázi
            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlexusDB.db");
            // získání objektu databáze
            db = new SQLiteConnection(databasePath);
            // Získání tabulky pro Uževatele
            db.CreateTable<Users>();
        }
        // funkce na zjištění jestli je někdo přihlášený
        public bool LookAtLocalLogin()
        {
            // Vezmi všechny uživatele
            var query = db.Table<Users>(); 
            // pokud je někdo  přihlášený tak poslat pravda, jinak lež 
            foreach (Users user in query)
            {
                if (user.IsLogged == true)
                {
                    return true;
                }
            }
            return false;
        }
        // Přihlaš zadaného uživatele a potvrď
        public bool LoginHim(Users Loginee)
        {
            // najdi uživatele, kteý má toto uživatelské jméno a heslo
            var query = db.Table<Users>().Where( v => v.Nickname.Equals(Loginee.Nickname) & v.Password.Equals(Loginee.Password));
            bool isLoginThere = false;
            // pokud něco takového existuje, tak přihlaš a vrať pravda. Jinak jen pošli lež.
            foreach (Users user in query)
            {
                isLoginThere = true;
                user.IsLogged = true;

                db.Update(user);
            }
            return isLoginThere;
        }
        // funkce na zadáníj nového uživatele
        public void RegisterHim(Users ToResgister)
        {

            db.Insert(ToResgister);
        }
        // funkce na odhlášní uživatele
        public void LogOutHim()
        {
            // Najdi uživatele který je přihlášený
            var query = db.Table<Users>().Where(v => v.IsLogged.Equals(true));
            // a odhlaš ho
            foreach (Users user in query)
            {
                user.IsLogged = false;

                db.Update(user);
            }
        }
        // najdu jestli zadáný přihlašovací jméno existuje 
        public bool TryUsername(string Username)
        {
            // najdi uživatele, který má zadaný uživatelký jméno
            var query = db.Table<Users>().Where(v => v.Nickname.Equals(Username));
            // pokud se najde poslat pravda, jinak lež
            foreach (Users user in query)
            {
                return true;
            }
            return false;
        }
        // Funkce na zíkání právě přihlášenýho uživatele
        public Users WhoLogged()
        {
            // základní objekt uživatele 
            Users who = new Users { Nickname = "Unknown", Id = -1, IsLogged = false, Password = "Unknown"};
            // najdi uživatele který jsou přihlášený
            var query = db.Table<Users>().Where(v => v.IsLogged.Equals(true));
            // pokud existuje,  dát do objektu a vrátit
            foreach (Users user in query)
            {
                who = user;
            }
            return who;
        }
    }
}
