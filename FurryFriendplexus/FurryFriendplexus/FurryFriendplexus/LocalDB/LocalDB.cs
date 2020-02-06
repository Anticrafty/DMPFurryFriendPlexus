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
                isLoginThere = true;
            }
            return isLoginThere;
        }
    }
}
