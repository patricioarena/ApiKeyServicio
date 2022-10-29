using Application.IFactory;
using Application.IServices;
using DataAccess.Models;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceKey : IServiceKey
    {
        private readonly DbContext _Context;
        private readonly IAbstractServiceFactory _Service;

        public ServiceKey(ApiKeyDbContext context, IAbstractServiceFactory service)
        {
            _Context = context;
            _Service = service;
        }

        private bool IsValidForAutoGrant(Guid apikey)
        {
            Guid guidKey = apikey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).Include(e => e.client).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException("Apikey is null or no exit");

            var keyEnable = key.enabled;
            var clientEnable = key.client.enabled;

            if ((keyEnable == true) && (clientEnable == true))
                return true;
            return false;
        }

        public List<Key> GetKeys()
        {
            return _Context.Set<Key>().Include(e => e.client).ToList();
        }

        public Key GetKey(int id)
        {
            Key key = _Context.Set<Key>().Where(e => e.id.Equals(id)).Include(e => e.client).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException($"Apikey id: { id } is null or no exit");

            return key;
        }

        // La key se genera automaticamente en la base de datos 
        // por defecto esta desactivada la debe dar de alta una persona
        // a futuro un proceso
        public AssingnedKeyDTO AssignKey(AssingnKeyDTO assignKeyDTO)
        {
            Key key = _Service.Mapper().Map<Key>(assignKeyDTO);
            _Context.Set<Key>().Add(key);
            _Context.SaveChanges();

            Key assingnedKey = _Context.Set<Key>().Where(e => e.id.Equals(key.id)).Include(e => e.client).FirstOrDefault();

            AssingnedKeyDTO assingnedKeyDTO = new AssingnedKeyDTO
            {
                clientId = assingnedKey.clientId,
                clientName = assingnedKey.client.client1,
                apiKey = assingnedKey.apiKey,
                ipStart = assingnedKey.ipStart,
                ipEnd = assingnedKey.ipEnd,
                enabled = assingnedKey.enabled
            };

            return assingnedKeyDTO;
        }
        
        public AssingnedKeyDTO Enable(AccessKeyDTO accessKeyDTO)
        {
            Guid guidKey = accessKeyDTO.apiKey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).Include(e => e.client).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException($"Apikey: { guidKey } is null or no exit");

            int clientId = accessKeyDTO.clientId;
            var client = _Context.Set<Client>().Where(e => e.id.Equals(clientId)).FirstOrDefault();

            if (client == null)
                throw new NullReferenceException("Client is null or no exit");

            bool isEnabled = false;
            if (key.clientId == client.id)
                isEnabled = true;

            key.enabled = isEnabled;


            _Context.Entry(key).State = EntityState.Modified;
            _Context.SaveChanges();

            Key assingnedKey = _Context.Set<Key>().Where(e => e.id.Equals(key.id)).Include(e => e.client).FirstOrDefault();

            AssingnedKeyDTO assingnedKeyDTO = new AssingnedKeyDTO
            {
                clientId = assingnedKey.clientId,
                clientName = assingnedKey.client.client1,
                apiKey = assingnedKey.apiKey,
                ipStart = assingnedKey.ipStart,
                ipEnd = assingnedKey.ipEnd,
                enabled = assingnedKey.enabled
            };

            return assingnedKeyDTO;
        }
        
        public int Disable(Guid key, string revoke_user)
        {
            var Update = _Context.Set<Key>().Where(e => e.apiKey.Equals(key)).FirstOrDefault();

            if (Update == null)
                throw new NullReferenceException("Apikey is null or no exit");

            Update.enabled = false;
            Update.dischargeDate = DateTime.Now;
            Update.revoke_user = revoke_user;
            _Context.Entry(Update).State = EntityState.Modified;
            return _Context.SaveChanges();
        }

        public BindedDTO GrantAppAccess(BindDTO bindDTO)
        {
            Guid guidKey = bindDTO.apiKey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).Include(e => e.client).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException($"Apikey: { guidKey } is null or no exit");

            int appId = bindDTO.applicationId;
            DataAccess.Models.Application app = _Context.Set<DataAccess.Models.Application>()
                    .Where(e => e.id.Equals(appId)).FirstOrDefault();

            if (app == null)
                throw new NullReferenceException($"ApplicationId: { appId } is null or no exit");

            bool isEnabled = IsValidForAutoGrant(guidKey);

            Key_Application key_Application = new Key_Application
            {
                clientId = key.client.id,
                applicationId = app.id,
                keyId = key.id,
                enabled = isEnabled
            };

            _Context.Set<Key_Application>().Add(key_Application);
            _Context.SaveChanges();

            BindedDTO bindedDTO = new BindedDTO
            {
                apiKey = guidKey,
                clientName = key.client.client1,
                nameApp = app.name,
                enabled = key_Application.enabled,
            };

            return bindedDTO;
        }

        public BindedDTO RevokeAppAccess(BindDTO bindDTO, string revoke_user)
        {
            Guid guidKey = bindDTO.apiKey;
            Key key = _Context.Set<Key>().Where(e => e.apiKey.Equals(guidKey)).Include(e => e.client).FirstOrDefault();

            if (key == null)
                throw new NullReferenceException($"Apikey: { guidKey } is null or no exit");

            int appId = bindDTO.applicationId;
            DataAccess.Models.Application app = _Context.Set<DataAccess.Models.Application>().Where(e => e.id.Equals(appId)).FirstOrDefault();

            if (app == null)
                throw new NullReferenceException($"ApplicationId: { appId } is null or no exit");

            Key_Application key_Application = _Context.Set<Key_Application>()
                .Where(e => e.applicationId.Equals(appId))
                .Where(e => e.keyId.Equals(key.id))
                .FirstOrDefault();
            
            if (key_Application == null)
                throw new NullReferenceException($"key: { guidKey } and App { appId } no linked");

            key_Application.revoke_user = revoke_user;
            key_Application.dischargeDate = DateTime.Now;
            key_Application.enabled = false;

            _Context.Entry(key_Application).State = EntityState.Modified;
            _Context.SaveChanges();

            BindedDTO bindedDTO = new BindedDTO
            {
                apiKey = guidKey,
                clientName = key.client.client1,
                nameApp = app.name,
                enabled = key_Application.enabled
            };

            return bindedDTO;
        }

    }
}
