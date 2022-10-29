using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Client
    {
        public Client()
        {
            Keys = new HashSet<Key>();
            Logs = new HashSet<Log>();
        }

        public int id { get; set; }
        public string client1 { get; set; }
        public DateTime created { get; set; }
        public DateTime? dischargeDate { get; set; }
        public bool enabled { get; set; }
        public string emiter_user { get; set; }
        public string revoke_user { get; set; }

        public virtual ICollection<Key> Keys { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
