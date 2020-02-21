using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    //Objekt vytvořený pro práci s Hdonocením
    public class Ratinging
    {
        // Identifikační číslo pro toto hodnocení
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        // Spojení se záznamem, který uživatel hodnotil,  pomocí ID záznamu
        public int RecordID { get; set; }

        // Spojení s Uživatelem, který hodnotilvzáznam,  pomocí ID Uživatele
        public int RaterUserID { get; set; }
        // Samotný procetnuální názor
        public int Rate { get; set; }
    }
}
