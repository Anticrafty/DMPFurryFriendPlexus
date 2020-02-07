using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FurryFriendplexus.Classes;

namespace FurryFriendplexus.LocalDB
{
    public class LocalDB
    {
        //https://github.com/praeclarum/sqlite-net/blob/master/README.md
        private string databasePath;
        private SQLiteConnection db;
        public LocalDB()
        {
            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlexusDB.db");
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Users>();
        }
        public bool LookAtLocalLogin()
        {
            var query = db.Table<Users>(); 
            bool isLoginThere = false;
            foreach (Users user in query)
            {
                if (user.IsLogged == true)
                {
                    isLoginThere = true;
                }
            }
            return isLoginThere;
        }
        public bool LoginHim(Users Loginee)
        {
            var query = db.Table<Users>().Where( v => v.Nickname.Equals(Loginee.Nickname) & v.Password.Equals(Loginee.Password));
            bool isLoginThere = false;
            foreach (Users user in query)
            {
                isLoginThere = true;
                user.IsLogged = true;

                db.Update(user);
            }
            return isLoginThere;
        }
        public void RegisterHim(Users ToResgister)
        {

            db.Insert(ToResgister);
        }

        public void LogOutHim()
        {
            var query = db.Table<Users>().Where(v => v.IsLogged.Equals(true));
            foreach (Users user in query)
            {
                user.IsLogged = false;

                db.Update(user);
            }
        }
        public bool TryUsername(string Username)
        {
            var query = db.Table<Users>().Where(v => v.Nickname.Equals(Username));
            foreach (Users user in query)
            {
                return true;
            }
            return false;
        }
        public string WhoLogged()
        {
            string who = "Unknown";
            var query = db.Table<Users>().Where(v => v.IsLogged.Equals(true));
            foreach (Users user in query)
            {
                who = user.Nickname;
            }
            return who;
        }
    }
}
