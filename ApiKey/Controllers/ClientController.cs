using System;
using System.Collections.Generic;
using System.Net;
using ApiKeyPOC.Results;
using Application.IServices;
using DataAccess.Models;
using Domain.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiKeyPOC.Controllers
{
#if DEBUG
    [AllowAnonymous]
#else
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    public class ClientController : CustomController
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IServiceClient _serviceClient;
        public ClientController(IServiceClient service, ILogger<ClientController> logger)
        {
            _logger = logger;
            _serviceClient = service;
        }

        [HttpGet("All")]
        public IActionResult GetClients()
        {
            try
            {
                List<Client> listClients = ImpersontedControllerAction(_serviceClient.GetClients);

                string message = "Returned all records!!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<List<Client>>(HttpStatusCode.OK, message, listClients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("ByClient/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                Client client = ImpersontedControllerAction(_serviceClient.GetClientById, id);

                string message = $"Returned record ::> { id } !!";
                _logger.LogInformation(message);
                return Ok(new ResponseApi<Client>(HttpStatusCode.OK, message, client));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ClientDto clientDto)
        {
            try
            {
                int? id = ImpersontedControllerAction(_serviceClient.Save, clientDto);
                JObject row_affected = new JObject();
                row_affected.Add("id", id);

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

        [HttpDelete("Disable/{id}")]
        public IActionResult Disable(int id)
        {
            try
            {
                int? number = ImpersontedControllerAction(_serviceClient.Disable, id, ImpersontedUser());
                JObject row_affected = new JObject();
                row_affected.Add("Row affected", number);

                string message = $"Revoke client ::> { id } Success!!";
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
