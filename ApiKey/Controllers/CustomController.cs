using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Principal;
using ApiKeyPOC.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace ApiKeyPOC.Controllers
{
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CustomController : Controller
    {
        public CustomController() { }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ObjectResult CustomErrorStatusCode(Exception e)
        {
            if (e is CustomException)
            {
                var errorCode = ((CustomException)e).errorCode;
                var message = ((CustomException)e).Message;
                return StatusCode((int)HttpStatusCode.PreconditionFailed, new ResponseApi<object>(HttpStatusCode.PreconditionFailed, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : message, errorCode));
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseApi<object>(HttpStatusCode.InternalServerError, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : e.Message));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tout>(Func< Tout> serviceMethod)
        {
            if (IsDebugWithDocker())
                return serviceMethod();
            
            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod();
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tin, Tout>(Func<Tin, Tout> serviceMethod, Tin param1)
        {
            if (IsDebugWithDocker())
                return serviceMethod(param1);

            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod(param1);
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tin, Tin2, Tout>(Func<Tin, Tin2, Tout> serviceMethod, Tin param1, Tin2 param2)
        {
            if (IsDebugWithDocker())
                return serviceMethod(param1, param2);

            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod(param1, param2);
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public string ImpersontedUser()
        {
            if (IsDebugWithDocker())
                return "Some developer in debug";

            var callerIdentity = User.Identity as WindowsIdentity;
            return callerIdentity.Name;
        }
        
        private static bool IsDebugWithDocker()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" 
                   && Environment.GetEnvironmentVariable("DOCKER_ENVIRONMENT") == "true";
        }
    }
}