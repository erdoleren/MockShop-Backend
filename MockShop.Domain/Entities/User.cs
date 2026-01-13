using System;
using System.Collections.Generic;
using System.Text;

namespace MockShop.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
