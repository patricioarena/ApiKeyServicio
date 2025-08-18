using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Referer
    {
        public int id { get; set; }
        public bool enabled { get; set; }
        public int clientId { get; set; }
        public string ipStart { get; set; }
        public string ipEnd { get; set; }
        public string name { get; set; }

        public virtual Client client { get; set; }
    }
}
