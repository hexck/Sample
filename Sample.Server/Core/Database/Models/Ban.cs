using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Server.Core.Database.Models
{
    public class Ban
    {
        [BsonId]
        public Guid Id { get; set; }

        public string IpAddress { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Issued { get; set; }

        public double Days { get; set; }
    }
}
