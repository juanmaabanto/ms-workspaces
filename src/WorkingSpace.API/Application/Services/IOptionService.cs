using System.Collections.Generic;
using System.Threading.Tasks;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public interface IOptionService
    {
        Task<OptionDto> GetOptionAsync(string optionId);
        Task<IEnumerable<OptionListDto>> GetOptionsAsync(string moduleId);
    }
}