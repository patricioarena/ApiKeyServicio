using Application.IFactory;
using Application.IServices;
using DataAccess.Models;
using Domain.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceClient : ServiceGeneral, IServiceClient
    {
        private readonly DbContext _Context;
        
        private readonly IAbstractServiceFactory _Service;

        public ServiceClient(ApiKeyDbContext context, IAbstractServiceFactory service)
        {
            _Context = context;
            _Service = service;
        }

        public List<Client> GetClients()
        {
            return _Context.Set<Client>().Include(e => e.Keys).ToList();
        }

        public Client GetClientById(int clientId)
        {
            return _Context.Set<Client>().Where(e => e.id.Equals(clientId)).Include(e => e.Keys).FirstOrDefault();
        }

        public int Save(ClientDto clientDto)
        {
            clientDto.client = NormalizeString(clientDto.client);
            Client client = _Service.Mapper().Map<Client>(clientDto);
            client.enabled = true;

            _Context.Set<Client>().Add(client);
            _Context.SaveChanges();
            int id = client.id;
            return id;
        }

        public int Disable(int clientId, string revoke_user)
        {
            var Update = _Context.Set<Client>().FirstOrDefault(e => e.id.Equals(clientId));

            if (Update == null)
                throw new NullReferenceException(Message.null_Client);

            Update.enabled = false;
            Update.dischargeDate = DateTime.Now;
            Update.revoke_user = revoke_user;
            _Context.Entry(Update).State = EntityState.Modified;
            return _Context.SaveChanges();
        }

        public bool CreateClientForAuthentica(ClientWithKeyDto reqDto)
        {
            using (var transaction = _Context.Database.BeginTransaction())
            {
                try
                {
                    // Crear el cliente
                    ClientDto clientDto = new ClientDto { client = reqDto.client };
                    clientDto.client = NormalizeString(clientDto.client);
                    
                    Client client = _Service.Mapper().Map<Client>(clientDto);
                    client.enabled = true;
                    _Context.Set<Client>().Add(client);
                    _Context.SaveChanges();

                    // Asignar la key al cliente creado
                    AssingnKeyDto assingnKeyDto = new AssingnKeyDto
                    {
                        clientId = client.id,
                        ipStart = reqDto.ipStart,
                        ipEnd = reqDto.ipEnd,
                        referer = reqDto.referer
                    };
                    
                    Key key = _Service.Mapper().Map<Key>(assingnKeyDto);
                    _Context.Set<Key>().Add(key);
                    _Context.SaveChanges();

                    // Commit de la transacción
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback en caso de error
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
