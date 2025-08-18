using System;
using System.Linq;
using ApiKeyPOC.Configs;
using Application.IServices;
using Application.Services.UseCase;
using DataAccess.Models;
using Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Application.Services
{
    /// <summary>
    /// Servicio encargado de la autenticación y validación de claves, clientes y aplicaciones,
    /// así como del registro de logs relacionados con la autenticación de API Keys.
    /// </summary>
    public class ServiceAuthentication : IServiceAuthentication
    {
        private readonly DbContext _context;

        private readonly IServiceLogApikeyDB _serviceLogApikeyDb;
        
        private readonly IAuthenticaConfig _authenticaConfig;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de autenticación.
        /// </summary>
        /// <param name="context">Contexto de base de datos para acceder a los datos.</param>
        /// <param name="serviceLog">Servicio para registrar logs de autenticación.</param>
        public ServiceAuthentication(ApiKeyDbContext context, IServiceLogApikeyDB serviceLog, IAuthenticaConfig  authenticaConfig)
        {
            _context = context;
            _serviceLogApikeyDb = serviceLog;
            _authenticaConfig = authenticaConfig;
        }

        /// <summary>
        /// Verifica si la clave API, cliente y aplicación asociados a la solicitud son válidos y tienen acceso autorizado.
        /// </summary>
        /// <param name="request">Datos de la solicitud de autenticación.</param>
        /// <returns>True si la clave, cliente, aplicación y rango de IP son válidos; de lo contrario, false.</returns>
        public bool VerificationKey(RequestDto request)
        {
            Key key = GetAndValidateKey(request);
            Client client = GetClient(request);
            ValidateKeyClientRelationship(key, client, request);
            DataAccess.Models.Application app = GetAndValidateApplication(request);
            Key_Application keyApp = GetAndValidateKeyApplication(key, app, client, request);
            Referer referer = _context.Set<Referer>().FirstOrDefault(e => e.clientId.Equals(key.id));

            
            bool isInRange = ValidateIpRange(referer, request);
            bool isValidKey = keyApp.key.enabled;
            bool isValidClient = keyApp.key.client.enabled;
            bool hasAccess = keyApp.enabled ?? false;

            return isValidKey && isValidClient && hasAccess && isInRange;
        }
        
        /// <summary>
        /// Verifica si el cliente de Authentica de una organización está autorizado, considerando la validez del Referer,
        /// la clave API, el cliente, la aplicación, el acceso y el rango de IP.
        /// </summary>
        /// <param name="values">Valores del header Referer de la petición HTTP.</param>
        /// <param name="request">Datos de la solicitud de autenticación.</param>
        /// <returns>True si todos los criterios de autorización se cumplen; de lo contrario, false.</returns>
        public bool VerificationKeyForAuthentica(StringValues values, RequestDto request)
        {
            Client client = GetClient(request);
            Referer referer = _context.Set<Referer>().Where(e => e.clientId.Equals(client.id)).FirstOrDefault();
            
            bool isRefValid = isRefererValid(referer, values);
            bool isInRange = _authenticaConfig.ValidRangeOn() ? 
                ValidateIpRange(referer, request) : 
                true;
            
            return isInRange && isRefValid && client.enabled;
        }

        /// <summary>
        /// Verifica si la dirección IP proporcionada se encuentra dentro del rango permitido para la clave API especificada.
        /// </summary>
        /// <param name="apiKey">Clave API a validar.</param>
        /// <param name="address">Dirección IP a verificar.</param>
        /// <returns>True si la dirección está dentro del rango permitido; de lo contrario, false.</returns>
        public bool IsInRange(Guid apiKey, string address)
        {
            Guid guidKey = apiKey;
            Key key = _context.Set<Key>().FirstOrDefault(e => e.apiKey.Equals(guidKey));
            Referer referer = _context.Set<Referer>().FirstOrDefault(e => e.clientId.Equals(key.id));

            return IsAddressRangeValid.Test(referer, address);
        }
        
        private bool isRefererValid(Referer referer, StringValues values)   
        {
            if (string.IsNullOrEmpty(values))
                return false;

            if (referer == null)
                return false;
            
            if (values.ToString().StartsWith(referer.name) && referer.enabled)
                return true;
            return false;
        }

        private Key GetAndValidateKey(RequestDto request)
        {
            var key = _context.Set<Key>().FirstOrDefault(e => e.apiKey.Equals(request.apiKey));
            if (key == null)
            {
                _serviceLogApikeyDb.LogDb(request, Message.null_Key);
                throw new NullReferenceException(Message.null_Key);
            }

            return key;
        }

        private Client GetClient(RequestDto request)
        {
            var client = _context.Set<Client>().FirstOrDefault(e => e.id.Equals(request.clientId));
            if (client == null)
            {
                _serviceLogApikeyDb.LogDb(request, Message.null_Client);
                throw new NullReferenceException(Message.null_Client);
            }

            return client;
        }

        private void ValidateKeyClientRelationship(Key key, Client client, RequestDto request)
        {
            if (!key.clientId.Equals(client.id))
            {
                _serviceLogApikeyDb.LogDb(request, Message.null_relationship);
                throw new NullReferenceException(Message.null_relationship);
            }
        }

        private DataAccess.Models.Application GetAndValidateApplication(RequestDto request)
        {
            var app = _context.Set<DataAccess.Models.Application>()
                .FirstOrDefault(e => e.id.Equals(request.appId));
            
            if (app == null)
            {
                _serviceLogApikeyDb.LogDb(request, Message.null_App);
                throw new NullReferenceException(Message.null_App);
            }

            return app;
        }

        private Key_Application GetAndValidateKeyApplication(Key key, DataAccess.Models.Application app, Client client, RequestDto request)
        {
            var keyApp = _context.Set<Key_Application>()
                .Where(e => e.clientId.Equals(client.id))
                .Where(e => e.applicationId.Equals(app.id))
                .Where(e => e.keyId.Equals(key.id))
                .Include(e => e.key)
                .Include(e => e.key.client)
                .FirstOrDefault();

            if (keyApp == null)
            {
                _serviceLogApikeyDb.LogDb(request, Message.null_relationship2);
                throw new NullReferenceException(Message.null_relationship2);
            }

            return keyApp;
        }

        private bool ValidateIpRange(Referer referer, RequestDto request)
        {
            var isInRange = IsAddressRangeValid.Test(referer, request.remoteIp);
            if (!isInRange)
                _serviceLogApikeyDb.LogDb(request, Message.ip_out_range);
            return isInRange;
        }
    }
}