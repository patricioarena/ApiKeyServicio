using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LogDTO
    {
        public int id { get; set; }
        public string clientName { get; set; }
        public int clientId { get; set; }
        public Guid apiKey { get; set; }
        public string applicationName { get; set; }
        public int applicationId { get; set; }
        public string remoteIp { get; set; }
        public DateTime? incidentDate { get; set; }
        public string description { get; set; }
    }
}
