using System;
namespace DemoApplication.Database.Models
{
    public class Address
    {
        public string? OrdererFirstName { get; set; }
        public string? OrdererLastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}