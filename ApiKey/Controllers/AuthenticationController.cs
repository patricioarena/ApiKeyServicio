using ApiKeyPOC.Results;
using Application.IServices;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiKeyPOC.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : CustomController
    {
        private readonly ILogger<AuthenticationController> _Logger;
        private readonly IServiceAuthentication _ServiceAuthentication;
        private readonly IHttpContextAccessor _Accessor;
        public AuthenticationController(IHttpContextAccessor accessor, IServiceAuthentication service, ILogger<AuthenticationController> logger) : base()
        {
            _Logger = logger;
            _ServiceAuthentication = service;
            _Accessor = accessor;
        }
     
        [AllowAnonymous]
        [HttpPost("VerificationKey")]
        public IActionResult VerificationKey([FromBody] RequestDTO requestDTO)
        {
            try
            {
                bool isValid = _ServiceAuthentication.VerificationKey(requestDTO);

                JObject row_affected = new JObject();
                row_affected.Add("isValid", isValid);

                string message = $"Verification Key ::> { isValid } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("IsInRange/{apikey}/{remoteIp}'Solo_para_Test'")]
        public IActionResult IsInRange(Guid apikey, string remoteIp)
        {
            try
            {
                bool isValid = _ServiceAuthentication.IsInRange(apikey, remoteIp);

                JObject row_affected = new JObject();
                row_affected.Add("IsInRange", isValid);
                
                string message = $"Is In Range ::> { isValid } !!";
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
