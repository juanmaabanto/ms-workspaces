using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Attributes;
using Sofisoft.Enterprise.SeedWork.MongoDB.Domain;

namespace Sofisoft.Accounts.WorkingSpace.API.Models
{
    [BsonCollection("workspace")]
    public class Workspace : Document
    {
        #region Properties

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("details")]
        public List<WorkspaceDetail> Details { get; set; }

        [BsonElement("domains")]
        public List<string> Domains { get; set; }

        #endregion

        #region Builders

        protected Workspace()
        {
            Details = new List<WorkspaceDetail>();
        }

        #endregion
        
    }
}