using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ReactTraining2023.Data.Models;
using ReactTraining2023.Services.Interfaces;

namespace ReactTraining2023.Controllers
{
    [ApiController]
    [Route("api/appscore")]
    public class AppScoreController : ControllerBase
	{
		private readonly IAppScoreService _appScoreService;

		public AppScoreController(IAppScoreService appScoreService)
		{
			_appScoreService = appScoreService;
        }

        [HttpGet("GetAllAppScore")]
        [ProducesResponseType(typeof(AppScore), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppScore(string projectName)
		{
			if (projectName == "")
			{
				return BadRequest();
			}

			var result = await _appScoreService.GetAllScoreByProjectName(projectName);

            return Ok(result);
		}

        [HttpPost("AddAppScore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAppScore([FromBody] AppScore newAppScore)
		{
			var result = await _appScoreService.AddScore(newAppScore);
			if (result != null)
			{
				return Ok(result);
			}

			return BadRequest("Invalid request body");
		}

        [HttpPut("UpdateAppScore")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAppScore([FromBody] AppScore updateAppScore)
        {
            var result = await _appScoreService.UpdateScore(updateAppScore);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Invalid request body");
        }

        [HttpDelete("DeleteAppScore/{appScoreId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAppScore(int appScoreId)
        {
            if (appScoreId <= 0)
            {
                return BadRequest("Invalid request body");
            }

            var result = await _appScoreService.DeleteScoreById(appScoreId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }
    }
}