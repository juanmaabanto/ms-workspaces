using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    public class MenuOption
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("optionId")]
        public string OptionId { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }
        
    }
}