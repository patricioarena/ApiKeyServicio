using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Key
    {
        public Key()
        {
            Key_Applications = new HashSet<Key_Application>();
        }

        public int id { get; set; }
        public int clientId { get; set; }
        public Guid apiKey { get; set; }
        public DateTime created { get; set; }
        public DateTime? dischargeDate { get; set; }
        public bool enabled { get; set; }
        public string emiter_user { get; set; }
        public string revoke_user { get; set; }
        public string ipStart { get; set; }
        public string ipEnd { get; set; }

        public virtual Client client { get; set; }
        public virtual ICollection<Key_Application> Key_Applications { get; set; }
    }
}
