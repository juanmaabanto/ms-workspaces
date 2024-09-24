using System;
using System.Collections.Generic;
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
    public class ModulesController : ControllerBase
    {
        private readonly IOptionService _optionService;

        public ModulesController(IOptionService optionService)
        {
            _optionService = optionService
                ?? throw new ArgumentNullException(nameof(optionService));
        }

        #region Gets

        /// <summary>
        /// Returns list of options of a module.
        /// </summary>
        /// <param name="moduleId">Module identifier.</param>
        [Authorize]
        [Route("{moduleId}/options")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OptionListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOptions(string moduleId)
        {
            try
            {
                var result = await _optionService.GetOptionsAsync(moduleId);

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