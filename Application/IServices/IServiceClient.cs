using DataAccess.Models;
using Domain.DTOs;
using System.Collections.Generic;

namespace Application.IServices
{
    public interface IServiceClient
    {
        Client GetClientById(int clientId);
        List<Client> GetClients();
        int Register(ClientDTO clientDTO);
        int Disable(int clientId, string revoke_user);
    }
}