using Application.IFactory;
using Application.IServices;
using DataAccess.Models;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceAuthentication : IServiceAuthentication
    {
        private readonly DbContext _Context;
        private readonly IAbstractServiceFactory _Service;

        private readonly IServiceLogApikeyDB _ServiceLogApikeyDB;
        public ServiceAuthentication(ApiKeyDbContext context, IAbstractServiceFactory service, IServiceLogApikeyDB serviceLog)
        {
            _Context = context;
            _Service = service;
            _ServiceLogApikeyDB = serviceLog;
        }

        public bool VerificationKey(RequestDTO request)
        {
            Guid guidKey = request.apiKey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).FirstOrDefault();

            if (key == null)
            {
                _ServiceLogApikeyDB.LogDb(request, Message.null_Key);
                throw new NullReferenceException(Message.null_Key);
            }

            int clientId = request.clientId;
            Client client = _Context.Set<Client>().Where(e => e.id.Equals(clientId)).FirstOrDefault();

            if (client == null)
            {
                _ServiceLogApikeyDB.LogDb(request, Message.null_Client);
                throw new NullReferenceException(Message.null_Client);
            }

            if (!key.clientId.Equals(clientId))
            {
                _ServiceLogApikeyDB.LogDb(request, Message.null_relationship);
                throw new NullReferenceException(Message.null_relationship);
            }

            int appId = request.appId;
            DataAccess.Models.Application app = _Context.Set<DataAccess.Models.Application>().Where(e => e.id.Equals(appId)).FirstOrDefault();

            if (app == null)
            {
                _ServiceLogApikeyDB.LogDb(request, Message.null_App);
                throw new NullReferenceException(Message.null_App);
            }

            Key_Application key_Application = _Context.Set<Key_Application>()
                .Where(e => e.clientId.Equals(request.clientId))
                .Where(e => e.applicationId.Equals(request.appId))
                .Where(e => e.keyId.Equals(key.id))
                .Include(e => e.key)
                .Include(e => e.key.client)
                .FirstOrDefault();

            if (key_Application == null)
            {
                _ServiceLogApikeyDB.LogDb(request, Message.null_relationship2);
                throw new NullReferenceException(Message.null_relationship2);
            }

            var IsInRange = true;

            // Descomentar cuando se requiera verificar
            // que el ip desde el que se realiza la peticion esta dentro
            // del rango establecido 

            //if ((key.ipStart == null) || (key.ipEnd == null))
            //{
            //    _ServiceLogApikeyDB.LogDb(request, Message.null_ip);
            //    throw new NullReferenceException(Message.null_ip);
            //}

            //IsInRange = this.IsInRange(key.ipStart, key.ipEnd, request.remoteIp);

            if (IsInRange.Equals(false))
                _ServiceLogApikeyDB.LogDb(request, Message.ip_out_range);

            var isValidKey = key_Application.key.enabled;
            var isValidClient = key_Application.key.client.enabled;
            var hasAccess = key_Application.enabled;

            if ((isValidKey == true) && (isValidClient == true) && (hasAccess == true) && (IsInRange == true))
                return true;
            return false;
        }

        public bool IsInRange(Guid apiKey, string address)
        {
            Guid guidKey = apiKey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException(Message.null_Key);

            if ((key.ipStart == null) || (key.ipEnd == null))
                throw new NullReferenceException(Message.null_ip);

            var IsInRange = this.IsInRange(key.ipStart, key.ipEnd, address);

            return IsInRange;
        }

        public bool IsInRange(string startIpAddr, string endIpAddr, string address)
        {
            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);
            long ip = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);
            return ip >= ipStart && ip <= ipEnd;
        }

    }
}
