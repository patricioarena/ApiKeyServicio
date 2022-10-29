using ApiKeyPOC.Results;
using Application.IServices;
using DataAccess.Models;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class KeyController : CustomController
    {
        private readonly ILogger<KeyController> _Logger;
        private readonly IServiceKey _ServiceKey;
        private readonly IHttpContextAccessor _Accessor;
        public KeyController(IHttpContextAccessor accessor, IServiceKey service, ILogger<KeyController> logger) : base()
        {
            _Logger = logger;
            _ServiceKey = service;
            _Accessor = accessor;
        }

        [HttpGet("All")]
        public IActionResult GetKeys()
        {
            try
            {
                List<Key> listKeys = ImpersontedControllerAction<List<Key>>(_ServiceKey.GetKeys);

                string message = "Returned all records!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<Key>>(HttpStatusCode.OK, message, listKeys));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpGet("ByKey/{id}")]
        public IActionResult GetKey(int id)
        {
            try
            {
                Key key = ImpersontedControllerAction<int, Key>(_ServiceKey.GetKey,id);

                string message = $"Returned key ::> { key.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<Key>(HttpStatusCode.OK, message, key));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        // Genera una key para un cliente
        [HttpPost("AssignKey")]
        public IActionResult AssignKey([FromBody] AssingnKeyDTO assingnKeyDTO)
        {
            try
            {
                AssingnedKeyDTO assingnedKey = ImpersontedControllerAction<AssingnKeyDTO, AssingnedKeyDTO>(_ServiceKey.AssignKey, assingnKeyDTO);

                string message = $"Assigned key ::> { assingnedKey.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<AssingnedKeyDTO>(HttpStatusCode.OK, message, assingnedKey));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Enable")]
        public IActionResult Enable([FromBody] AccessKeyDTO assingnKeyDTO)
        {
            try
            {
                AssingnedKeyDTO assingnedKey = ImpersontedControllerAction<AccessKeyDTO, AssingnedKeyDTO>(_ServiceKey.Enable, assingnKeyDTO);

                string message = $"Enable key ::> { assingnedKey.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<AssingnedKeyDTO>(HttpStatusCode.OK, message, assingnedKey));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpDelete("Disable/{key}")]
        public IActionResult Disable(Guid key)
        {
            try
            {
                int? number = ImpersontedControllerAction<Guid, string, int>(_ServiceKey.Disable, key, ImpersontedUser());
                JObject row_affected = new JObject();
                row_affected.Add("Row affected", number);

                string message = $"Revoke key ::> { key } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("GrantAppAccess")]
        public IActionResult GrantAppAccess([FromBody] BindDTO bindDTO)
        {
            try
            {
                BindedDTO bindedDTO = ImpersontedControllerAction<BindDTO, BindedDTO>(_ServiceKey.GrantAppAccess, bindDTO);

                string message = $"Grant app access key ::> { bindedDTO.apiKey } & { bindedDTO.nameApp } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<BindedDTO>(HttpStatusCode.OK, message, bindedDTO));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("RevokeAppAccess")]
        public IActionResult RevokeAppAccess([FromBody] BindDTO bindDTO)
        {
            try
            {
                BindedDTO bindedDTO = ImpersontedControllerAction<BindDTO, string, BindedDTO>(_ServiceKey.RevokeAppAccess, bindDTO, ImpersontedUser());

                string message = $"Revoke app access key ::> { bindedDTO.apiKey } & { bindedDTO.nameApp } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<BindedDTO>(HttpStatusCode.OK, message, bindedDTO));
            }
            catch (System.Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }


    }
}
