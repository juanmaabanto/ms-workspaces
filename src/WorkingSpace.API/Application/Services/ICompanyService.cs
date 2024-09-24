using System.Threading.Tasks;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;

namespace Sofisoft.Accounts.WorkingSpace.API.Application.Services
{
    public interface ICompanyService
    {
        Task<CompanyDataDto> GetCompanyAsync();
    }
}