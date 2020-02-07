using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FurryFriendplexus.Classes
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public string Nickname { get; set; }

        public string Password { get; set; }

        public bool IsLogged { get; set; }
}
}
