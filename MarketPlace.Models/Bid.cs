using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Bid : IAuditableEntity
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public int AuctionProductId { get; set; }
        public int SellerCompanyId { get; set; }
        public int SellerUserId { get; set; }
        public decimal OfferedAmount { get; set; }
        public BidStatus Status { get; set; } = BidStatus.None;

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Auction? Auction { get; set; }
        public AuctionProduct? AuctionProduct { get; set; }
        public User? SellerCompany { get; set; }
    }
}
