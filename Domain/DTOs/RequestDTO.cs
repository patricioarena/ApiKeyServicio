using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RequestDTO
    {
        public int appId { get; set; }
        public int clientId { get; set; }
        public Guid apiKey { get; set; }
        public string remoteIp { get; set; }
    }
}
