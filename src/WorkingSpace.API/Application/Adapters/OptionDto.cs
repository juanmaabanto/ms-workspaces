using System.Collections.Generic;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Adapters
{
    public class OptionDto
    {
        public string OptionId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<ActionDto> Actions { get; set; }
        
    }
}