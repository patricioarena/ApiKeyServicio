using System;
using System.ComponentModel.DataAnnotations;

namespace ApiKey.Whitelist
{
    public class EntryEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }  // Could be IP, email, or other identifier

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
