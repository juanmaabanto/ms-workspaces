using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("menu")]
    public class Menu : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("moduleId")]
        public string ModuleId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("parentId")]
        [BsonIgnoreIfNull]
        public string ParentId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        [BsonElement("collapsible")]
        public bool Collapsible { get; set; }

        [BsonElement("icon")]
        [BsonIgnoreIfNull]
        public string Icon { get; set; }

        [BsonElement("options")]
        [BsonIgnoreIfNull]
        public List<MenuOption> Options { get; set; }

    }
}