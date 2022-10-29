using ApiKeyPOC.Results;
using Application.IServices;
using Application.Services;
using DataAccess.Models;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiKeyPOC.Controllers
{
#if DEBUG
    [AllowAnonymous]
#else
#endif
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class LogsController : CustomController
    {
        private readonly ILogger<LogsController> _Logger;
        private readonly IServiceLogApikeyDB _ServiceLogApikeyDB;
        public LogsController(IServiceLogApikeyDB service, ILogger<LogsController> logger) : base()
        {
            _Logger = logger;
            _ServiceLogApikeyDB = service;
        }

        [HttpGet("All")]
        public IActionResult GetApps()
        {
            try
            {
                List<LogDTO> listLogs = ImpersontedControllerAction<List<LogDTO>>(_ServiceLogApikeyDB.GetLogs);

                string message = "Returned all records!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<LogDTO>>(HttpStatusCode.OK, message, listLogs));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }
        
        [HttpGet("GetLogs/ByClient/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                List<LogDTO> listLogs = ImpersontedControllerAction<int, List<LogDTO>>(_ServiceLogApikeyDB.ByClient, id);

                string message = $"Returned all record for ::> { id } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<LogDTO>>(HttpStatusCode.OK, message, listLogs));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("GetLogs/ByClientKey/{apikey}")]
        public IActionResult GetClientById(Guid apikey)
        {
            try
            {
                List<LogDTO> listLogs = ImpersontedControllerAction<Guid, List<LogDTO>>(_ServiceLogApikeyDB.ByClientKey, apikey);

                string message = $"Returned all record for ::> { apikey } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<LogDTO>>(HttpStatusCode.OK, message, listLogs));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }
    }
}
