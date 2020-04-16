using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Database.Models
{
    public class License
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Key { get; set; }

        public string Hwid { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Issued { get; set; }
        
        public int ExpireAfterDays { get; set; }
    }
}
