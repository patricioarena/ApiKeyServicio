using System;
using System.ComponentModel.DataAnnotations;

namespace ApiKey.Whitelist
{
    public class EntryDto
    {
        [Required]
        [MaxLength(255)]
        public string Value { get; set; }  // Could be IP, email, or other identifier

        public string Description { get; set; }

    }
}
