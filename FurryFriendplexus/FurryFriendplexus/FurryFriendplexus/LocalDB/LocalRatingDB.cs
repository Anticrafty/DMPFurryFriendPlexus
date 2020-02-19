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
        #region Saves
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
        public void SaveRating(Ratinging NewRating)
        {
            db.Insert(NewRating);
        }
        public void SaveNewName(Namies newName)
        {
            db.Insert(newName);
        }
        #endregion
        #region Update
        public void RecordHaveRegistered(Record Selected)
        {
            db.Update(Selected);
        }
        public void UpdateRating(Ratinging inputRatinging)
        {
            db.Update(inputRatinging);
        }


        #endregion
        #region Get One By his ID
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
        public Namies GetName(int IDNamed)
        {
            var query = db.Table<Namies>().Where(v => v.RecordID.Equals(IDNamed));
            foreach(Namies name in query)
            {
                return name;
            }
            return null;
        }
        #endregion
        #region Get Many by his ID
        #endregion
        #region Get one by others id

        public int GetUsersIDFromNamesID(int RecordID)
        {
            int UsersID = -1;
            var query = db.Table<Record>().Where(v => v.Id.Equals(RecordID));
            Record OthersRecord = new Record();
            foreach (Record finded in query)
            {
                OthersRecord = finded;
            }
            if (OthersRecord.IsLinkedToUSer)
            {
                UsersID = OthersRecord.LinkedUserID;
            }
            return UsersID;
        }
        public Record GetUsersRecord(int UsersID)
        {
            var query = db.Table<Record>().Where(v => v.LinkedUserID.Equals(UsersID));
            foreach (Record User in query)
            {
                return User;
            }
            return null;
        }

        #endregion
        #region Get many by others ID
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
        public List<Ratinging> GetUsersRatings(int UsersID)
        {
            var query = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(UsersID));
            List<Ratinging> ItsRatings = new List<Ratinging>();
            foreach (Ratinging ratiee in query)
            {
                ItsRatings.Add(ratiee);
            }
            return ItsRatings;
        }
        #endregion
        #region Get All
        public List<Namies> GelAllNames()
        {
            var query = db.Table<Namies>();
            List<Namies> Output = new List<Namies>();
            foreach (Namies namies in query)
            {
                Output.Add(namies);
            }
            return Output;
        }
        #endregion
        #region Specific
        public List<Record> GetStartingRecords(int UserID)
        {
            var query1 = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(UserID));
            var query2 = db.Table<Record>();
            List<Record> Output = new List<Record>();
            foreach (Record record in query2)
            {
                bool AlredyRated = false;
                foreach (Ratinging ratingOfUser in query1)
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
        public Ratinging GetUsersRatingOfRecord(int selectedID, int selectorID)
        {
            var query = db.Table<Ratinging>().Where(v => v.RaterUserID.Equals(selectorID) & v.RecordID.Equals(selectedID));

            Ratinging Output = null;
            foreach (Ratinging ratiee in query)
            {
                Output = ratiee;
            }
            return Output;
        }
        public bool HaveUserRecord(Users user)
        {
            var query = db.Table<Record>();
            bool isRecordThere = false;
            foreach (Record record in query)
            {
                if (record.LinkedUserID == user.Id)
                {
                    isRecordThere = true;
                }
            }
            return isRecordThere;
        }
        #endregion
    }
}