using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FurryFriendplexus.Classes;

namespace FurryFriendplexus.LocalDB
{
    // Objekt pro práci s částí databáze určený pro Záznamy a jejich jména a hodnocení
    public class LocalRatingDB
    {   
        //cesta pro lokální databázy 
        private string databasePath;
        // Samotný objek databáze
        private SQLiteConnection db;
        public LocalRatingDB()
        {
            // Zjištění cesty pro databázi
            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlexusDB.db");
            // získání objektu databáze
            db = new SQLiteConnection(databasePath);
            // Získání tabulky pro Záznamy
            db.CreateTable<Record>();
            // Získání tabulky pro jména
            db.CreateTable<Namies>();
            // Získání tabulky pro hodnocení
            db.CreateTable<Ratinging>();
        }
        // Skupina funkcí určená pro ukládání nových objektů
        #region Saves
        // funkce určená pro uložení nového zadaného záznamu a jeho věcí od již zaregistrovaného uživatele
        public void SaveNewRecord(Record Newbie,List<Namies> NewNames, Ratinging NewRating)
        {
            // Uložení nového záznamu
            db.Insert(Newbie);
            // získání údajů z Tabulky Záznamů
            var qeury = db.Table<Record>();
            // získat poslední uložený záznam pro jeho ID
            foreach (Record record in qeury)
            {
                Newbie = record;
            }
            // uložení všech zadaných jmen
            foreach (Namies NewName in NewNames)
            {
                // Zadaní ID záznamu k jménům
                NewName.RecordID = Newbie.Id;
                db.Insert(NewName);
            }
            // přidání IDzáznamu k hodnocení na to
            NewRating.RecordID = Newbie.Id;
            // ukládání toho hodnocení
            SaveRating(NewRating);
        }
        // funkce určená pro uložení nového zadaného záznamu a jeho jmen pro zrovna se registrujícího uživatele
        public void SaveNewUsersRecord(Record Newbie, List<Namies> NewNames)
        {
            db.Insert(Newbie);
            var qeury = db.Table<Record>();
            foreach (Record record in qeury)
            {
                Newbie = record;
            }
            foreach (Namies NewName in NewNames)
            {
                NewName.RecordID = Newbie.Id;
                db.Insert(NewName);
            }
        }
        // Funkce pro ukládání nového honocení od již existujícího záznamu
        public void SaveRating(Ratinging NewRating)
        {
            db.Insert(NewRating);
        }
        // Funkce pro ukládání nového jména od již existujícího záznamu
        public void SaveNewName(Namies newName)
        {
            db.Insert(newName);
        }
        #endregion
        // Skupina funcí určená k přepisování objektů
        #region Update
        // přiřazování Záznamu k právě se registrujícímu se Uživately
        public void RecordHaveRegistered(Record Selected)
        {
            db.Update(Selected);
        }
        // přepisovaní hodnocení
        public void UpdateRating(Ratinging inputRatinging)
        {
            db.Update(inputRatinging);
        }


        #endregion
        // Skupina funkcí určená k získávání jednoho objektu pomocí jeho ID
        #region Get One By his ID
        // Funkce na získávaní konkrétního záznamu
        public Record FindRecord(int IDSuspect)
        {
            // Nalezení z záznamu z jeho tabulky, přez jeho ID
            var qeury = db.Table<Record>().Where(v => v.Id.Equals(IDSuspect));
            // Pošli získanej záznam z vyhledávání
            foreach (Record record in qeury)
            {
                return record;
            }
            // k tomuhle by nemělo dojít
            return null;
        }
        
        #endregion
        // Skupina funkcí určená k zíkávání mnoho objektů pomocí jejich ID
        #region Get Many by his ID
        #endregion
        // Skupina funkcí určená k získávání jednoho objektů pomocí ID jinýho objektu
        #region Get one by others id
        // funkce na získání ID Uživatele pomocí id Záznamu, kte kterému patří
        public int GetUsersIDFromNamesID(int RecordID)
        {
            //vytvoření objektu pro ID
            int UsersID = -1;
            // Nalezení konkrétního Záznamu 
            var query = db.Table<Record>().Where(v => v.Id.Equals(RecordID));
            // Vytvoření objektu pro tento záznam
            Record OthersRecord = new Record();
            foreach (Record finded in query)
            {
                // vložení toho záznamu do jeho objektu
                OthersRecord = finded;
            }
            // Jetsli je záznam přiřazený k nějakému uživatel, tak dát jeho ID do jeho objektu
            if (OthersRecord.IsLinkedToUSer)
            {
                UsersID = OthersRecord.LinkedUserID;
            }
            return UsersID;
        }
        //funkce na získání Záznamu podle ID Usera
        public Record GetUsersRecord(int UsersID)
        {
            // získání Záznamu podle ID Usera
            var query = db.Table<Record>().Where(v => v.LinkedUserID.Equals(UsersID));
            // Vracení záznamu 
            foreach (Record User in query)
            {
                return User;
            }
            // k tomuhle by nemělo dojít
            return null;
        }
        // Funkce na získávaní konkrétního Jména dle jeho záznamu
        public Namies GetName(int IDNamed)
        {
            var query = db.Table<Namies>().Where(v => v.RecordID.Equals(IDNamed));
            foreach (Namies name in query)
            {
                return name;
            }
            return null;
        }

        #endregion
        // Skupina funkcí určna k získání mnoho objektů pomocí ID jinýho objektu
        #region Get many by others ID
        //Funkce na získání jmen patřící Jednomu záznamu
        public List<Namies> GetNames(int RecordID)
        {
            // najít jména které patří k zadanýmu záznamu
            var query = db.Table<Namies>().Where(v => v.RecordID.Equals(RecordID));
            // objek pro tyto jména
            List<Namies> Output = new List<Namies>();
            // zadání jmen do jejich objektu
            foreach (Namies namies in query)
            {
                Output.Add(namies);
            }
            return Output;
        }
        // Funkce na získání hodnocení vytvořený jedním uživatelem
        public List<Ratinging> GetUsersRatings(int UsersID)
        {
            // získání hodnocení vytvořený zadaným uživatelem
            var query = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(UsersID));
            // Objekt pro tyto hodnocení 
            List<Ratinging> ItsRatings = new List<Ratinging>();
            // zadání hodnocení do jejich objektu
            foreach (Ratinging ratiee in query)
            {
                ItsRatings.Add(ratiee);
            }
            return ItsRatings;
        }
        #endregion
        // seznam funkcí na získání všech záznamů jednohodruhu objektu
        #region Get All
        //funkcí na získání všech jmen
        public List<Namies> GelAllNames()
        {
            // Získat jména z tabulky
            var query = db.Table<Namies>();
            // Objekt na tyto jména
            List<Namies> Output = new List<Namies>();
            // Vložení jmen do tohoto objektu
            foreach (Namies namies in query)
            {
                Output.Add(namies);
            }
            return Output;
        }
        #endregion
        // seznam funkcí které nejdou tak lehce zařadit
        #region Specific
        // funkce získání záznamů které uživatel ještě nehodnotil
        public List<Record> GetStartingRecords(int UserID)
        {
            // získej HOdnocení zadaného uživatele
            var query1 = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(UserID));
            // získej všechny záznany
            var query2 = db.Table<Record>();
            // objekt pro vycházející záznamy
            List<Record> Output = new List<Record>();
            // dát do objektu všechny záznymu 
            foreach (Record record in query2)
            {
                //které ještě nemají hodnocení od tohoto uživatele
                // Zjisti jestli byl hodnocen
                bool AlredyRated = false;
                foreach (Ratinging ratingOfUser in query1)
                {
                    if (ratingOfUser.RecordID == record.Id)
                    {
                        AlredyRated = true;
                    }

                }
                // pokud ještě nebyl hodnocen vlož do objektu
                if (!AlredyRated)
                {
                    Output.Add(record);
                }
            }
            return Output;
        }
        // získej Hodnocení Uživatele na toto hodnocení
        public Ratinging GetUsersRatingOfRecord(int selectedID, int selectorID)
        {
            // získej z tabulky hodnocení zadaného záznamu od zadaného uživatele
            var query = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(selectorID) & v.RecordID.Equals(selectedID));
            // vrať hodnocení co jsi našel
            foreach (Ratinging ratiee in query)
            {
                return ratiee;
            }
            // pokud nic není, tak vrať nic
            return null;
        }
        // funkce na kontrolu jestli Uživatel má svůj záznam
        public bool HaveUserRecord(Users user)
        {
            // Vezmi všechny Záznamy
            var query = db.Table<Record>();
            bool isRecordThere = false;

            // Jestli najde záznam tak pošle pravda
            foreach (Record record in query)
            {
                if (record.LinkedUserID == user.Id)
                {
                    isRecordThere = true;
                }
            }
            // pokud ne, tak vrátí lež.
            return isRecordThere;
        }
        #endregion
    }
}