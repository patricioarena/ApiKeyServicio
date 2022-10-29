using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AssingnedKeyDTO
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public Guid apiKey { get; set; }
        public string ipStart { get; set; }
        public string ipEnd { get; set; }
        public bool enabled { get; set; }
    }
}
