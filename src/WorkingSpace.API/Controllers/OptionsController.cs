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
    public class OptionsController : ControllerBase
    {
        private readonly IOptionService _optionService;

        public OptionsController(IOptionService optionService)
        {
            _optionService = optionService
                ?? throw new ArgumentNullException(nameof(optionService));
        }

        #region Gets

        /// <summary>
        /// Returns option by id.
        /// </summary>
        /// <param name="optionId">Option identifier.</param>
        [Authorize]
        [Route("{optionId}")]
        [HttpGet]
        [ProducesResponseType(typeof(OptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOption(string optionId)
        {
            try
            {
                var result = await _optionService.GetOptionAsync(optionId);

                return Ok(result);
            }
            catch (WorkingSpaceDomainException ex)
            {
                return BadRequest(new ErrorViewModel(ex.ErrorId, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        #endregion
    }
}