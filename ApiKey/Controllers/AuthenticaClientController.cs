using System;
using System.Net;
using ApiKeyPOC.Results;
using Application.IServices;
using Domain.DTOs;
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
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticaClientController : CustomController
    {
        private static readonly string Referer = "Referer";

        private readonly IHttpContextAccessor _Accessor;
        
        private readonly ILogger<AuthenticationController> _Logger;

        private readonly IServiceAuthentication _ServiceAuthentication;

        private readonly IServiceClient _ServiceClient;

        public AuthenticaClientController(IHttpContextAccessor accessor, IServiceAuthentication serviceAuthentication, 
            IServiceClient serviceClient, ILogger<AuthenticationController> logger)
        {
            _ServiceAuthentication = serviceAuthentication;
            _ServiceClient = serviceClient;
            _Accessor = accessor;
            _Logger = logger;
        }

        [HttpGet("Validate/Client/{clientId}")]
        public IActionResult Get(int clientId)
        {
            try
            {
                ConnectionInfo connectionInfo = _Accessor.HttpContext.Connection;
                RequestDTO requestDTO = new RequestDTO
                {
                    appId = 1,
                    clientId = clientId,
                    apiKey = new Guid("56ae433e-f36b-1410-89dd-007170650b78"),
                    remoteIp = connectionInfo.RemoteIpAddress.MapToIPv4().ToString(),
                };

                bool isValid = false;
                if (_Accessor.HttpContext.Request.Headers.TryGetValue(Referer, out StringValues values))
                {
                     isValid = _ServiceAuthentication.VerificationKeyForAuthentica(values, requestDTO);
                }

                JObject row_affected = new JObject();
                row_affected.Add("isValid", isValid);

                string message = $"Verification Key ::> { isValid } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ClientWithKeyDTO clientDTO)
        {
            try
            {
                bool isCreated = ImpersontedControllerAction(_ServiceClient.CreateClientForAuthentica, clientDTO);
                JObject row_affected = new JObject();
                row_affected.Add("isCreated", isCreated);

                string message = $"Created new client ::> { isCreated } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }
    }
}
