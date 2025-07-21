using System;
using Domain.DTOs;
using Microsoft.Extensions.Primitives;

namespace Application.IServices
{
    /// <summary>
    /// Interfaz para el servicio de autenticación y validación de API Keys, clientes y aplicaciones.
    /// </summary>
    public interface IServiceAuthentication
    {
        
        /// <summary>
        /// Verifica si la clave API, cliente y aplicación asociados a la solicitud son válidos y tienen acceso autorizado.
        /// </summary>
        /// <param name="request">Datos de la solicitud de autenticación.</param>
        /// <returns>True si la clave, cliente, aplicación y rango de IP son válidos; de lo contrario, false.</returns>
        bool VerificationKey(RequestDTO request);

        /// <summary>
        /// Verifica si el cliente de Authentica de una organización está autorizado, considerando la validez del Referer,
        /// la clave API, el cliente, la aplicación, el acceso y el rango de IP.
        /// </summary>
        /// <param name="values">Valores del header Referer de la petición HTTP.</param>
        /// <param name="request">Datos de la solicitud de autenticación.</param>
        /// <returns>True si todos los criterios de autorización se cumplen; de lo contrario, false.</returns>
        bool VerificationKeyForAuthentica(StringValues values, RequestDTO request);
        
        /// <summary>
        /// Verifica si la dirección IP proporcionada se encuentra dentro del rango permitido para la clave API especificada.
        /// </summary>
        /// <param name="apiKey">Clave API a validar.</param>
        /// <param name="address">Dirección IP a verificar.</param>
        /// <returns>True si la dirección está dentro del rango permitido; de lo contrario, false.</returns>
        bool IsInRange(Guid apiKey, string address);
    }
}
