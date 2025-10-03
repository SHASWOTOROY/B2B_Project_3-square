using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class OrderProduct : IAuditableEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public List<ProductVariation> Variations { get; set; } = new List<ProductVariation>();
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Product? Product { get; set; }
    }
}
