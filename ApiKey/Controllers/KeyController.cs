using ApiKeyPOC.Results;
using Application.IServices;
using DataAccess.Models;
using Domain.Data;
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
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
#endif
    [Route("api/[controller]")]
    public class KeyController : CustomController
    {
        private readonly ILogger<KeyController> _Logger;
        private readonly IServiceKey _ServiceKey;
        private readonly IHttpContextAccessor _Accessor;
        public KeyController(IHttpContextAccessor accessor, IServiceKey service, ILogger<KeyController> logger)
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
                List<Key> listKeys = ImpersontedControllerAction(_ServiceKey.GetKeys);

                string message = "Returned all records!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<List<Key>>(HttpStatusCode.OK, message, listKeys));
            }
            catch (Exception ex)
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
                Key key = ImpersontedControllerAction(_ServiceKey.GetKey,id);

                string message = $"Returned key ::> { key.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<Key>(HttpStatusCode.OK, message, key));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        // Genera una key para un cliente
        [HttpPost("AssignKey")]
        public IActionResult AssignKey([FromBody] AssingnKeyDto assingnKeyDto)
        {
            try
            {
                AssingnedKeyDto assingnedKey = ImpersontedControllerAction(_ServiceKey.AssignKey, assingnKeyDto);

                string message = $"Assigned key ::> { assingnedKey.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<AssingnedKeyDto>(HttpStatusCode.OK, message, assingnedKey));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("Enable")]
        public IActionResult Enable([FromBody] AccessKeyDto assingnKeyDto)
        {
            try
            {
                AssingnedKeyDto assingnedKey = ImpersontedControllerAction(_ServiceKey.Enable, assingnKeyDto);

                string message = $"Enable key ::> { assingnedKey.apiKey } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<AssingnedKeyDto>(HttpStatusCode.OK, message, assingnedKey));
            }
            catch (Exception ex)
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
                int? number = ImpersontedControllerAction(_ServiceKey.Disable, key, ImpersontedUser());
                JObject row_affected = new JObject();
                row_affected.Add("Row affected", number);

                string message = $"Revoke key ::> { key } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, message, row_affected));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("GrantAppAccess")]
        public IActionResult GrantAppAccess([FromBody] BindDto bindDto)
        {
            try
            {
                BindedDto bindedDto = ImpersontedControllerAction(_ServiceKey.GrantAppAccess, bindDto);

                string message = $"Grant app access key ::> { bindedDto.apiKey } & { bindedDto.nameApp } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<BindedDto>(HttpStatusCode.OK, message, bindedDto));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }

        [HttpPost("RevokeAppAccess")]
        public IActionResult RevokeAppAccess([FromBody] BindDto bindDto)
        {
            try
            {
                BindedDto bindedDto = ImpersontedControllerAction(_ServiceKey.RevokeAppAccess, bindDto, ImpersontedUser());

                string message = $"Revoke app access key ::> { bindedDto.apiKey } & { bindedDto.nameApp } Success!!";
                _Logger.LogInformation(message);
                return Ok(new ResponseApi<BindedDto>(HttpStatusCode.OK, message, bindedDto));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                return CustomErrorStatusCode(ex);
            }
        }


    }
}
