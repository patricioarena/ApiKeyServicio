using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class BindedDTO
    {
        public Guid apiKey { get; set; }
        public string clientName { get; set; }
        public string nameApp { get; set; }
        public bool? enabled { get; set; }

    }
}
