using System.Collections.Generic;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Adapters
{
    public class RouteDto
    {
        public string OptionId { get; set; }
        public string HostName { get; set; }
        public string HostUri { get; set; }
        public List<string> Paths { get; set; }
    }
}