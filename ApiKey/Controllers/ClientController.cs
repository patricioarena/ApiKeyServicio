using ApiKeyPOC.Results;
using Application.IServices;
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
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    public class ClientController : CustomController
    {
        private readonly ILogger<ClientController> _Logger;
        private readonly IServiceClient _ServiceClient;
        public ClientController(IServiceClient service, ILogger<ClientController> logger)
        {
            _Logger = logger;
            _ServiceClient = service;
        }

        [HttpGet("All")]
        public IActionResult GetClients()
        {
            try
            {
                List<Client> listClients = ImpersontedControllerAction(_ServiceClient.GetClients);

                string message = "Returned all records!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<Client>>(HttpStatusCode.OK, message, listClients));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("ByClient/{id}")]
        public IActionResult GetClientById(int id)
        {
            try
            {
                Client client = ImpersontedControllerAction(_ServiceClient.GetClientById,id);

                string message = $"Returned record ::> { id } !!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<Client>(HttpStatusCode.OK, message, client));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Register")]
        public IActionResult SetClient([FromBody] ClientDTO clientDTO)
        {
            try
            {
                int? id = ImpersontedControllerAction(_ServiceClient.Save, clientDTO);
                JObject row_affected = new JObject();
                row_affected.Add("id", id);

                string message = $"Insert client ::> { id } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpDelete("Disable/{id}")]
        public IActionResult Disable(int id)
        {
            try
            {
                int? number = ImpersontedControllerAction(_ServiceClient.Disable, id, ImpersontedUser());
                JObject row_affected = new JObject();
                row_affected.Add("Row affected", number);

                string message = $"Revoke client ::> { id } Success!!";
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
