using ApiKeyPOC.Results;
using Application.IServices;
using Application.Services;
using DataAccess.Models;
using Domain.Data;
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
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    public class AppsController : CustomController
    {
        private readonly ILogger<AppsController> _logger;
        private readonly IServiceApplication _serviceApps;
        public AppsController(IServiceApplication service, ILogger<AppsController> logger)
        {
            _logger = logger;
            _serviceApps = service;
        }

        [HttpGet("All")]
        public IActionResult GetApps()
        {
            try
            {
                List<DataAccess.Models.Application> listApps = ImpersontedControllerAction(_serviceApps.GetApps);

                string message = "Returned all records!!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<List<DataAccess.Models.Application>>(HttpStatusCode.OK, message, listApps));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("ByApp/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                DataAccess.Models.Application app = ImpersontedControllerAction(_serviceApps.GetAppById,id);

                string message = $"Returned record ::> { id } !!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<DataAccess.Models.Application>(HttpStatusCode.OK, message, app));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ApplicationDto appDto)
        {
            try
            {
                int? id = ImpersontedControllerAction(_serviceApps.Save, appDto);
                JObject row_affected = new JObject();
                row_affected.Add("id", id.ToString());

                string message = $"Insert client ::> { id } Success!!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

 

    }
}
