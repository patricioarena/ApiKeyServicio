using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Data
{
    public class BindDto
    {
        public Guid apiKey { get; set; }
        public int applicationId { get; set; }
    }
}
