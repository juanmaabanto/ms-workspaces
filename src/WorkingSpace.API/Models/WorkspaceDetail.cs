using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    public class WorkspaceDetail
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("moduleId")]
        public string ModuleId { get; set; }

    }
}