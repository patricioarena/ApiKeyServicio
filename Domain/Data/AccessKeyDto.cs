using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.Data
{
    public class AccessKeyDto
    {
        public Guid apiKey { get; set; }
        public int clientId { get; set; }
    }
}
