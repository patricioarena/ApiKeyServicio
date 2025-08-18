using System;
using System.Net;
using ApiKeyPOC.Results;
using Application.IServices;
using Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ApiKeyPOC.Controllers
{
    /// <summary>
    /// Controlador para verificar la autenticidad de una API Key. Es consumido desde la librería apikey.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticationController : CustomController
    {
        private readonly ILogger<AuthenticationController> _logger;
        
        private readonly IServiceAuthentication _serviceAuthentication;
        
        /// <summary>
        /// Inicializa una nueva instancia del controlador de autenticación.
        /// </summary>
        /// <param name="service">Servicio de autenticación de API Keys.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        public AuthenticationController(IServiceAuthentication service, ILogger<AuthenticationController> logger)
        {
            _logger = logger;
            _serviceAuthentication = service;
        }
        
        //TODO: revisar logica de este endpoint
        /// <summary>
        /// Verifica la autenticidad de una API Key recibida en el cuerpo de la solicitud.
        /// </summary>
        /// <param name="requestDto">Datos de la solicitud de autenticación.</param>
        /// <returns>Resultado HTTP con el estado de la autenticidad de la API Key.</returns>
        [AllowAnonymous]
        [HttpPost("VerificationKey")]
        public IActionResult VerificationKey([FromBody] RequestDto requestDto)
        {
            try
            {
                bool isValid = _serviceAuthentication.VerificationKey(requestDto);

                JObject responseData = new JObject();
                responseData.Add("isValid", isValid);

                string message = $"Verification Key ::> { isValid } !!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, responseData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("IsInRange/{apikey}/{remoteIp}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult IsInRange(Guid apikey, string remoteIp)
        {
            try
            {
                bool isValid = _serviceAuthentication.IsInRange(apikey, remoteIp);

                JObject responseData = new JObject();
                responseData.Add("IsInRange", isValid);
                
                string message = $"Is In Range ::> { isValid } !!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, responseData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

    }
}
