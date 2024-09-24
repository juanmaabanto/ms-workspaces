using System.Collections.Generic;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Adapters
{
    public class AppDataDto
    {
        public IEnumerable<ModuleWorkspaceDto> Modules { get; set; }
        public IEnumerable<RouteDto> Routes { get; set; }
    }
}