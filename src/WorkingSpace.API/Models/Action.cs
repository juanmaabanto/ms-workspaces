using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("action")]
    public class Action : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("resourceId")]
        public string ResourceId { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
    }
}