using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Company : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public ICollection<Product>? Products { get; set; } 
    }
}
