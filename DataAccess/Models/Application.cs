using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Application
    {
        public Application()
        {
            Key_Applications = new HashSet<Key_Application>();
            Logs = new HashSet<Log>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public virtual ICollection<Key_Application> Key_Applications { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
