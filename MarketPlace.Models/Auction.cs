using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Auction : IAuditableEntity
    {
        public int Id { get; set; }
        public int BuyerCompanyId { get; set; }
        public int BuyerUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AuctionStatus Status { get; set; } = AuctionStatus.None;
        public int? DeliveryAddressId { get; set; }
        public Address? DeliveryAddress { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Company? BuyerCompany { get; set; }
        public ICollection<AuctionProduct>? AuctionProducts { get; set; }
    }
}
