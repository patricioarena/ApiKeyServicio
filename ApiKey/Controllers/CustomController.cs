using ApiKeyPOC.Results;
using Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Net;
using System.Security.Principal;

namespace ApiKeyPOC.Controllers
{
#if DEBUG
    [AllowAnonymous]
#else
#endif
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = IISDefaults.AuthenticationScheme)]
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
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ResponseApi<object>(HttpStatusCode.InternalServerError, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : e.Message));
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tout>(Func< Tout> serviceMethod)
        {
            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod();
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tin, Tout>(Func<Tin, Tout> serviceMethod, Tin param1)
        {
            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod(param1);
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public Tout ImpersontedControllerAction<Tin, Tin2, Tout>(Func<Tin, Tin2, Tout> serviceMethod, Tin param1, Tin2 param2)
        {
            Tout result = default(Tout);
            var callerIdentity = User.Identity as WindowsIdentity;
            WindowsIdentity.RunImpersonated(callerIdentity.AccessToken, () =>
            {
                result = serviceMethod(param1, param2);
            });
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar la compatibilidad de la plataforma", Justification = "<pendiente>")]
        public string ImpersontedUser()
        {
            var callerIdentity = User.Identity as WindowsIdentity;
            return callerIdentity.Name;
        }
    }
}