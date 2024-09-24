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
    public class WorkspacesController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspacesController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService
                ?? throw new ArgumentNullException(nameof(workspaceService));
        }

        #region Gets

        /// <summary>
        /// Returns information for the application.
        /// </summary>
        [Authorize]
        [Route("appdata")]
        [HttpGet]
        [ProducesResponseType(typeof(AppDataDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAppData()
        {
            try
            {
                var workspaceId = await _workspaceService.GetWorkspaceIdAsync();
                var tModules = _workspaceService.GetModulesAsync(workspaceId);
                var tRoutes = _workspaceService.GetRoutesAsync(workspaceId);

                await Task.WhenAll(tModules, tRoutes);

                return Ok(new AppDataDto {
                    Modules = tModules.Result,
                    Routes = tRoutes.Result 
                });
            }
            catch (WorkingSpaceDomainException ex)
            {
                return BadRequest(new ErrorViewModel(ex.ErrorId, ex.Message));
            }
        }

        #endregion
    }
}