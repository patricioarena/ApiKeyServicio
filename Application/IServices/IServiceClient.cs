using DataAccess.Models;
using Domain.Data;
using System.Collections.Generic;

namespace Application.IServices
{
    public interface IServiceClient
    {
        Client GetClientById(int clientId);
        List<Client> GetClients();
        int Save(ClientDto clientDto);
        int Disable(int clientId, string revoke_user);
        bool CreateClientForAuthentica(ClientWithKeyDto clientDto);
    }
}