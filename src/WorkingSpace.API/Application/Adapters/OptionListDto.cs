using System.Collections.Generic;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Adapters
{
    public class OptionListDto
    {
        public string OptionId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; }
        public bool Leaf { get; set; }
        public bool Collapsible { get; set; }
        public List<string> Paths { get; set; }
    }
}