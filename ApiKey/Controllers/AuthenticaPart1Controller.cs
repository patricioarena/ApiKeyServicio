using System;
using System.Net;
using ApiKeyPOC.Configs;
using ApiKeyPOC.Results;
using Application.IServices;
using Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace ApiKeyPOC.Controllers
{
    /// <summary>
    /// Controlador para validar la autenticación de clientes Authentica
    /// usando Referer, rangos de ips y key por default.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public partial class AuthenticaController : CustomController
    {
        private const string Referer = "Referer";
        
        private readonly IHttpContextAccessor _accessor;
        
        private readonly ILogger<AuthenticationController> _logger;

        private readonly IServiceAuthentication _serviceAuthentication;

        private readonly IServiceClient _serviceClient;
        
        private readonly IAuthenticaConfig _authenticaConfig;

        public AuthenticaController(IHttpContextAccessor accessor, IServiceAuthentication serviceAuthentication, 
            IServiceClient serviceClient, ILogger<AuthenticationController> logger, IAuthenticaConfig authenticaConfig)
        {
            _serviceAuthentication = serviceAuthentication;
            _serviceClient = serviceClient;
            _accessor = accessor;
            _logger = logger;
            _authenticaConfig = authenticaConfig;
        }

        [HttpGet("Validate/Client/{clientId}")]
        public IActionResult Get(int clientId)
        {
            try
            {
                ConnectionInfo connectionInfo = _accessor.HttpContext.Connection;
                RequestDto requestDto = new RequestDto
                {
                    appId = _authenticaConfig.GetApplicationId(),
                    clientId = clientId,
                    remoteIp = connectionInfo.RemoteIpAddress.MapToIPv4().ToString(),
                };

                bool isValid = false;
                if (_accessor.HttpContext.Request.Headers.TryGetValue(Referer, out StringValues values))
                {
                     isValid = _serviceAuthentication.VerificationKeyForAuthentica(values, requestDto);
                }

                JObject responseData = new JObject();
                responseData.Add("isValid", isValid);

                string message = $"Verification client is valid ::> { isValid } !!!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, responseData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                JObject responseData = new JObject();
                responseData.Add("isValid", false);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Verification client is valid ::> false !!", responseData));
            }
        }
        
        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ClientWithKeyDto clientDto)
        {
            try
            {
                bool isCreated = ImpersontedControllerAction(_serviceClient.CreateClientForAuthentica, clientDto);
                JObject row_affected = new JObject();
                row_affected.Add("isCreated", isCreated);

                string message = $"Created new client ::> { isCreated } !!!";
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
