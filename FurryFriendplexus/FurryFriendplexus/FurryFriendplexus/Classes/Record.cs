using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    public class Record
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public string Race { get; set; }
        public int LinkedUserID { get; set; }
        public bool IsLinkedToUSer { get; set; }
    }
}
