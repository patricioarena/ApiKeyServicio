using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Log
    {
        public int id { get; set; }
        public int clientId { get; set; }
        public Guid apiKey { get; set; }
        public int applicationId { get; set; }
        public string remoteIp { get; set; }
        public DateTime? incidentDate { get; set; }
        public string description { get; set; }

        public virtual Application application { get; set; }
        public virtual Client client { get; set; }
    }
}
