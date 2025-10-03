using Threesquare.Core.Models;

namespace MarketPlace.Models
{
    public class Order : IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int BuyerCompanyId { get; set; }
        public int SellerCompanyId { get; set; }
        public int BuyerUserId { get; set; }
        public int SellerUserId { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int? DeliveryAddressId { get; set; }
        public Address? DeliveryAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.None;

        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public Company? BuyerCompany { get; set; }
        public Company? SellerCompany { get; set; }
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
