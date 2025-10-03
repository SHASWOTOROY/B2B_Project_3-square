using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class AuctionProduct : IAuditableEntity
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int ProductId { get; set; }
        public int? AcceptedBidId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsBidAccepted { get { return AcceptedBidId != null; } }
        public List<ProductVariation> Variations { get; set; } = new List<ProductVariation>();

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Product? Product { get; set; }
        public Bid? AcceptedBid { get; set; }
    }
}
