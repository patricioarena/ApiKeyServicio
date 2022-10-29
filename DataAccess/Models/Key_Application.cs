using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Key_Application
    {
        public int id { get; set; }
        public int? clientId { get; set; }
        public int? applicationId { get; set; }
        public int? keyId { get; set; }
        public DateTime enabledDate { get; set; }
        public DateTime? dischargeDate { get; set; }
        public bool? enabled { get; set; }
        public string emiter_user { get; set; }
        public string revoke_user { get; set; }

        public virtual Application application { get; set; }
        public virtual Key key { get; set; }
    }
}
