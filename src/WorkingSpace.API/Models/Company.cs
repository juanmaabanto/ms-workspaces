using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("company")]
    public class Company : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("workspaceId")]
        public string WorkspaceId { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("tin")]
        public string Tin { get; set; }

        [BsonElement("businessName")]
        public string BusinessName { get; set; }

        [BsonElement("tradeName")]
        public string TradeName { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("cancelled")]
        public bool Cancelled { get; set; }
    }
}