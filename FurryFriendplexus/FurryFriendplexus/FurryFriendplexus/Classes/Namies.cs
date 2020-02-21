using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    //Objekt vytvořený pro práci s jménama záznamů
    public class Namies
    {
        [PrimaryKey, AutoIncrement]
        // Identifikační číslo pro tyto jména
        public int Id { get; set; }
        [Indexed]
        // Spojení se záznamem pomocí ID záznamu
        public int RecordID { get; set; }
        // Samotné jméno
        public string Name { get; set; }

    }
}
