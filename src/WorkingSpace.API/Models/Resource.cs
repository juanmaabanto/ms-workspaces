using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("resource")]
    public class Resource : Document
    {
        [BsonElement("name")]
        public string Name { get; set; }
        
    }
}