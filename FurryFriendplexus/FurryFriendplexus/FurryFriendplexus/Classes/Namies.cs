using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    public class Namies
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int RecordID { get; set; }
        public string Name { get; set; }

    }
}
