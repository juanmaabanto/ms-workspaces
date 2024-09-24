using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("module")]
    public class Module : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("clientAppId")]
        public string ClientAppId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("shortName")]
        public string ShortName { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("options")]
        [BsonIgnoreIfNull]
        public List<MenuOption> Options { get; set; }

        [BsonElement("resources")]
        [BsonIgnoreIfNull]
        public List<string> Resources { get; set; }
    }
}