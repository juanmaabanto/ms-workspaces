using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sofisoft.Accounts.WorkingSpace.API.Application.Adapters;
using Sofisoft.Accounts.WorkingSpace.API.Application.Services;
using Sofisoft.Accounts.WorkingSpace.API.Infrastructure.Exceptions;
using Sofisoft.Accounts.WorkingSpace.API.ViewModels;

namespace Sofisoft.Accounts.WorkingSpace.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService
                ?? throw new ArgumentNullException(nameof(companyService));
        }

        #region Gets

        /// <summary>
        /// Returns company data from the session.
        /// </summary>
        [Authorize]
        [Route("session")]
        [HttpGet]
        [ProducesResponseType(typeof(CompanyDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBySession()
        {
            try
            {
                var result = await _companyService.GetCompanyAsync();

                return Ok(result);
            }
            catch (WorkingSpaceDomainException ex)
            {
                return BadRequest(new ErrorViewModel(ex.ErrorId, ex.Message));
            }
        }

        #endregion

    }
}