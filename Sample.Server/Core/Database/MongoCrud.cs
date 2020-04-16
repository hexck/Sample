using Sample.Server.Core.Database.Models;
using Sample.Server.Core.Network;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Database
{
    public class MongoCrud
    {
        private IMongoDatabase Database { get; }

        public MongoCrud(string name=Constants.DbName)
        {
            Database = new MongoClient().GetDatabase(name);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = Database.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public void InsertRecords<T>(string table, T[] records)
        {
            var collection = Database.GetCollection<T>(table);
            collection.InsertMany(records);
        }
        public void InsertRecords<T>(string table, List<T> records)
        {
            InsertRecords(table, records.ToArray());
        }
        public List<T> RetrieveRecords<T>(string table)
        {
            var collection = Database.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public bool DeleteLicenseByHwid(string hwid)
        {
            var collection = Database.GetCollection<License>("Licenses");
            var deleteFilter = Builders<License>.Filter.Eq("Hwid", hwid);
            return collection.DeleteOne(deleteFilter).DeletedCount > 0;
        }

        public bool DeleteLicenseByKey(string key)
        {
            var collection = Database.GetCollection<License>("Licenses");
            var deleteFilter = Builders<License>.Filter.Eq("Key", key);
            return collection.DeleteOne(deleteFilter).DeletedCount > 0;
        }

        public void UpdateLicense(string key, License replacement)
        {
            var collection = Database.GetCollection<License>("Licenses");
            var deleteFilter = Builders<License>.Filter.Eq("Key", key);
            collection.FindOneAndReplace(deleteFilter, replacement);
        }

        public bool RevokeBan(Ban ban)
        {
            var collection = Database.GetCollection<Ban>("Bans");
            var deleteFilter = Builders<Ban>.Filter.Eq("Id", ban.Id);
            return collection.DeleteOne(deleteFilter).IsAcknowledged;
        }

        public bool RevokeBan(string ip)
        {
            var collection = Database.GetCollection<Ban>("Bans");
            var deleteFilter = Builders<Ban>.Filter.Eq("IpAddress", ip);
            return collection.DeleteOne(deleteFilter).IsAcknowledged;
        }

        public void Ban(string ip, int days = 7)
        {
            InsertRecord("Bans", new Ban { Id = Guid.NewGuid(), IpAddress = ip, Issued = DateTime.Now, Days = days });
        }

    }
}
