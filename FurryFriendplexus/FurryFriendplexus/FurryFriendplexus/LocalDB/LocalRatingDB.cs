using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FurryFriendplexus.Classes;

namespace FurryFriendplexus.LocalDB
{
    public class LocalRatingDB
    {
        private string databasePath;
        private SQLiteConnection db;
        public LocalRatingDB()
        {
            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PlexusDB.db");
            db = new SQLiteConnection(databasePath);
            db.CreateTable<Record>();
            db.CreateTable<Namies>();
            db.CreateTable<Ratinging>();
        }
        public void SaveNewRecord(Record Newbie,List<Namies> NewNames, Ratinging NewRating)
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
            NewRating.RecordID = Newbie.Id;
            SaveRating(NewRating);
        }
        public void SaveRating(Ratinging NewRating)
        {
            db.Insert(NewRating);
        }

        public Record FindRecord(int IDSuspect)
        {
            var qeury = db.Table<Record>().Where(v => v.Id.Equals(IDSuspect));
            Record Finded = new Record();
            foreach (Record record in qeury)
            {
                Finded = record;
            }
            return Finded;
        }

        public List<Record> GetStartingRecords(int UserID)
        {
            var query1 = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(UserID));
            var query2 = db.Table<Record>();
            List<Record> Output = new List<Record>();
            foreach (Record record in query2)
            {
                bool AlredyRated = false;
                foreach(Ratinging ratingOfUser in query1)
                {
                    if (ratingOfUser.RecordID == record.Id)
                    {
                        AlredyRated = true;
                    }
                    
                }
                if (!AlredyRated)
                {
                    Output.Add(record);
                }
            }
            return Output;
        }
        public List<Namies> GetNames(int RecordID)
        {
            var query = db.Table<Namies>().Where(v => v.RecordID.Equals(RecordID));
            List<Namies> Output = new List<Namies>();
            foreach (Namies namies in query)
            {
                Output.Add(namies);
            }
            return Output;
        }
    }
}