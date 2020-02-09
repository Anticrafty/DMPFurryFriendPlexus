using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    public class Ratinging
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public int RecordID { get; set; }
        public int RaterUserID { get; set; }
        public int Rate { get; set; }
    }
}
