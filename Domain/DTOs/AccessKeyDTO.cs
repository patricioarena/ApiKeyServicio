using System;
using System.Collections.Generic;

#nullable disable

namespace Domain.DTOs
{
    public class AccessKeyDTO
    {
        public Guid apiKey { get; set; }
        public int clientId { get; set; }
    }
}
