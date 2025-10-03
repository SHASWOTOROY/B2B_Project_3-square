using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Product : IAuditableEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public decimal MRPPrice { get; set; }
        public IList<ProductVariationType> Variations { get; set; } = new List<ProductVariationType>();

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Category? Category { get; set; }
        public Brand? Brand { get; set; }
        public ICollection<Company>? Companies { get; set; }
    }
}
