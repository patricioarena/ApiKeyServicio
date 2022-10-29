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
    public class AppsController : CustomController
    {
        private readonly ILogger<AppsController> _Logger;
        private readonly IServiceApplication _ServiceApps;
        public AppsController(IServiceApplication service, ILogger<AppsController> logger) : base()
        {
            _Logger = logger;
            _ServiceApps = service;
        }

        [HttpGet("All")]
        public IActionResult GetApps()
        {
            try
            {
                List<DataAccess.Models.Application> listApps = ImpersontedControllerAction<List<DataAccess.Models.Application>>(_ServiceApps.GetApps);

                string message = "Returned all records!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<DataAccess.Models.Application>>(HttpStatusCode.OK, message, listApps));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("ByApp/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                DataAccess.Models.Application app = ImpersontedControllerAction<int, DataAccess.Models.Application>(_ServiceApps.GetAppById,id);

                string message = $"Returned record ::> { id } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<DataAccess.Models.Application>(HttpStatusCode.OK, message, app));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ApplicationDTO appDTO)
        {
            try
            {
                int? id = ImpersontedControllerAction<ApplicationDTO, int>(_ServiceApps.Register, appDTO);
                JObject row_affected = new JObject();
                row_affected.Add("id", id.ToString());

                string message = $"Insert client ::> { id } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

 

    }
}
