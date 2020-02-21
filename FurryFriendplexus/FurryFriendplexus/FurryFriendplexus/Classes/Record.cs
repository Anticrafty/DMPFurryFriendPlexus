using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurryFriendplexus.Classes
{
    //Objekt vytvořený pro práci se záznamem
    public class Record
    {
        [PrimaryKey, AutoIncrement]
        // Identifikační číslo pro tento záznam
        public int Id { get; set; }
        [Indexed]
        // Živočnišný druh tohoto záznamu
        public string Race { get; set; }
        // Spojení s Uživatelem, kterýmu by mohl patřit tento záznam
        public int LinkedUserID { get; set; }
        // Jestli existuje nějaký uživatel kterému patří tento záznam
        public bool IsLinkedToUSer { get; set; }
    }
}
