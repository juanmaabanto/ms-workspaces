using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("option")]
    public class Option : Document
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("hostName")]
        public string HostName { get; set; }

        [BsonElement("hostUri")]
        public string HostUri { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("actions")]
        public List<string> Actions { get; set; }

        [BsonElement("paths")]
        public List<string> Paths { get; set; }
    }
}