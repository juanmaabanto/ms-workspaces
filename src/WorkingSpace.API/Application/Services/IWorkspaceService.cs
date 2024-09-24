using System.Collections.Generic;
using System.Threading.Tasks;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public interface IWorkspaceService
    {
        Task<IEnumerable<ModuleWorkspaceDto>> GetModulesAsync(string workspaceId);
        Task<IEnumerable<RouteDto>> GetRoutesAsync(string workspaceId);
        Task<string> GetWorkspaceIdAsync();
    }
}