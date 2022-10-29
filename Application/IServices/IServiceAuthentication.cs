using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IServiceAuthentication
    {
        bool VerificationKey(RequestDTO request);
        bool IsInRange(Guid apiKey, string address);
        bool IsInRange(string startIpAddr, string endIpAddr, string address);
    }
}
