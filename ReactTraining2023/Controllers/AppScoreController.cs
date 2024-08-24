using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ReactTraining2023.Data.Models;
using ReactTraining2023.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using ReactTraining2023.Hubs;

namespace ReactTraining2023.Controllers
{
    [ApiController]
    [Route("api/appscore")]
    public class AppScoreController : ControllerBase
	{
		private readonly IAppScoreService _appScoreService;
        private readonly string _SESSIONID = "sessionId";
        private string _sessionIdConfigValue = "";
        private string _sessionSignalRConfigValue = "";

        private readonly ScoreHub _scoreHub;

        private readonly ILogger<AppScoreController> _logger;

        public string SessionIdConfigValue
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionIdConfigValue)) {
                    var config = new ConfigurationBuilder()
                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                                                .AddEnvironmentVariables()
                                                .Build();

                    if (config != null)
                    {
                        _sessionIdConfigValue = config.GetSection("SessionId").Value;
                        _sessionSignalRConfigValue = config.GetSection("SignalRSessionId").Value;
                    }
                                           
                }
                    
                return _sessionIdConfigValue;
            }
        }

        public string SignalRSessionIdConfigValue
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionSignalRConfigValue))
                {
                    var config = new ConfigurationBuilder()
                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                                                .AddEnvironmentVariables()
                                                .Build();

                    if (config != null)
                    {
                        _sessionSignalRConfigValue = config.GetSection("SignalRSessionId").Value;
                    }

                }

                return _sessionSignalRConfigValue;
            }
        }

        public AppScoreController(IAppScoreService appScoreService, ILogger<AppScoreController> logger, ScoreHub scoreHub)
		{
			_appScoreService = appScoreService;
            _scoreHub = scoreHub;

            _logger = logger;
        }

        [HttpGet("GetAllAppScore")]
        [ProducesResponseType(typeof(AppScore), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAppScore(string projectName)
		{
            if (!IsMatchHeaderKey())
                return Unauthorized();

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
            if (!IsMatchHeaderKey())
                return Unauthorized();

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
            if (!IsMatchHeaderKey())
                return Unauthorized();

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
            if (!IsMatchHeaderKey())
                return Unauthorized();

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

        private bool IsMatchHeaderKey()
        {
            if (!Request.Headers.TryGetValue("_SESSIONID", out var sessionHeader))
                return false;

            if (string.IsNullOrEmpty(sessionHeader) || sessionHeader.ToString().ToLower() != SessionIdConfigValue.ToLower())
                return false;

            return true;
        }

        private bool IsMatchSignalRHeaderKey()
        {
            if (!Request.Headers.TryGetValue("_SESSIONID", out var sessionHeader))
                return false;

            if (string.IsNullOrEmpty(sessionHeader) || sessionHeader.ToString().ToLower() != SignalRSessionIdConfigValue.ToLower())
                return false;

            return true;
        }

        [HttpDelete("ClearScoreHub")]
        public async Task<IActionResult> ClearScoreHub()
        {
            if (!IsMatchSignalRHeaderKey())
                return Unauthorized();

            _scoreHub.ClearAllScoreHub();
            return Ok("All data in ScoreHub are cleared.");
        }

    }
}