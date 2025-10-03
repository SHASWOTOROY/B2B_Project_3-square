using System.Security.Claims;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuctionController : ControllerBase
{
    private readonly BaseDatabaseContext _db;
    private readonly IConfiguration _config;
    public AuctionController(BaseDatabaseContext db, IConfiguration config) { _db = db; _config = config; }

    private bool TryGetUser(out int userId, out int companyId)
    {
        userId = 0; companyId = 0;
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var cid = User.FindFirstValue("company_id") ?? User.FindFirstValue("companyId") ?? User.FindFirstValue("companyid");
        return int.TryParse(sub, out userId) && int.TryParse(cid, out companyId);
    }

    public record AddressIn(string FirstLine, string SecondLine, string City, string State, string PostalCode, decimal Longitude, decimal Latitude);
    public record VariationSelection(string TypeName, string ValueName);
    public record CreateAuctionRequest(int ProductId, int Quantity, DateTime? StartTimeUtc, DateTime? EndTimeUtc, AddressIn? DeliveryAddress, List<VariationSelection>? Variations);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuctionRequest request, CancellationToken ct)
    {
        if (!TryGetUser(out var buyerUserId, out var buyerCompanyId)) return Unauthorized();

        // Validate product and ownership (not required to own)
        var product = await _db.Set<Product>().FirstOrDefaultAsync(p => p.Id == request.ProductId, ct);
        if (product == null) return BadRequest("Invalid ProductId");

        var start = request.StartTimeUtc ?? DateTime.UtcNow;
        var end = request.EndTimeUtc ?? start.AddMinutes(30);
        if (end <= start) end = start.AddMinutes(1);
        if ((end - start) > TimeSpan.FromMinutes(30)) end = start.AddMinutes(30);

        // Optional delivery address
        Address? address = null;
        if (request.DeliveryAddress != null)
        {
            var a = request.DeliveryAddress;
            address = new Address
            {
                FirstLine = a.FirstLine,
                SecondLine = a.SecondLine,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                Longitude = a.Longitude,
                Latitude = a.Latitude
            };
            await _db.Set<Address>().AddAsync(address, ct);
            await _db.SaveChangesAsync(ct);
        }

        var auction = new Auction
        {
            BuyerCompanyId = buyerCompanyId,
            BuyerUserId = buyerUserId,
            StartTime = start,
            EndTime = end,
            Status = AuctionStatus.Open,
            DeliveryAddressId = address?.Id
        };
        await _db.Set<Auction>().AddAsync(auction, ct);
        await _db.SaveChangesAsync(ct);

        // Build auction product with selected variations (by type/value names)
        var selections = new List<ProductVariation>();
        if (request.Variations != null)
        {
            foreach (var sel in request.Variations)
            {
                var tName = sel.TypeName.Trim();
                var vName = sel.ValueName.Trim();
                var type = await _db.Set<VariationType>().FirstOrDefaultAsync(t => t.Name.ToLower() == tName.ToLower(), ct);
                if (type == null)
                {
                    type = new VariationType { Name = tName, IsActive = true };
                    await _db.Set<VariationType>().AddAsync(type, ct);
                    await _db.SaveChangesAsync(ct);
                }
                var value = await _db.Set<VariationValue>().FirstOrDefaultAsync(v => v.VariationTypeId == type.Id && v.Name.ToLower() == vName.ToLower(), ct);
                if (value == null)
                {
                    value = new VariationValue { VariationTypeId = type.Id, Name = vName, IsActive = true };
                    await _db.Set<VariationValue>().AddAsync(value, ct);
                    await _db.SaveChangesAsync(ct);
                }
                selections.Add(new ProductVariation { TypeId = type.Id, ValueId = value.Id, Type = type.Name, Value = value.Name });
            }
        }

        var ap = new AuctionProduct
        {
            AuctionId = auction.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = request.Quantity,
            Variations = selections
        };
        await _db.Set<AuctionProduct>().AddAsync(ap, ct);
        await _db.SaveChangesAsync(ct);

        return Ok(new { auction.Id, auction.Status, apId = ap.Id });
    }

    // Open auctions for sellers to see and bid
    [HttpGet("open")]
    public async Task<ActionResult<IEnumerable<object>>> GetOpen(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var items = await _db.Set<Auction>()
            .Where(a => a.Status == AuctionStatus.Open && a.StartTime <= now && a.EndTime >= now)
            .SelectMany(a => _db.Set<AuctionProduct>().Where(ap => ap.AuctionId == a.Id)
                .Select(ap => new
                {
                    a.Id,
                    a.BuyerCompanyId,
                    a.StartTime,
                    a.EndTime,
                    DeliveryAddress = a.DeliveryAddressId != null ? new
                    {
                        a.DeliveryAddress!.FirstLine,
                        a.DeliveryAddress!.SecondLine,
                        a.DeliveryAddress!.City,
                        a.DeliveryAddress!.State,
                        a.DeliveryAddress!.PostalCode,
                        a.DeliveryAddress!.Longitude,
                        a.DeliveryAddress!.Latitude
                    } : null,
                    ap.AuctionId,
                    AuctionProductId = ap.Id,
                    ap.ProductId,
                    ap.ProductName,
                    ap.Quantity,
                    Variations = ap.Variations.Select(v => new { v.TypeId, v.Type, v.ValueId, v.Value })
                }))
            .OrderBy(x => x.EndTime)
            .ToListAsync(ct);
        return Ok(items);
    }

    public record PlaceBidRequest(decimal UnitPrice);

    [HttpPost("{auctionId:int}/products/{auctionProductId:int}/bid")]
    public async Task<IActionResult> PlaceBid(int auctionId, int auctionProductId, [FromBody] PlaceBidRequest request, CancellationToken ct)
    {
        if (!TryGetUser(out var sellerUserId, out var sellerCompanyId)) return Unauthorized();
        var now = DateTime.UtcNow;

        var auction = await _db.Set<Auction>().FirstOrDefaultAsync(a => a.Id == auctionId, ct);
        if (auction == null) return NotFound("Auction not found");
        if (auction.Status != AuctionStatus.Open || auction.StartTime > now || auction.EndTime < now)
            return BadRequest("Auction not accepting bids now");
        var allowSelfBid = _config.GetValue<bool>("Auctions:AllowSelfBid");
        if (!allowSelfBid && auction.BuyerCompanyId == sellerCompanyId)
            return BadRequest("You cannot bid on your own company's auction");

        var ap = await _db.Set<AuctionProduct>().FirstOrDefaultAsync(x => x.Id == auctionProductId && x.AuctionId == auctionId, ct);
        if (ap == null) return NotFound("Auction product not found");

        var total = request.UnitPrice * ap.Quantity;
        var bid = new Bid
        {
            AuctionId = auctionId,
            AuctionProductId = auctionProductId,
            SellerCompanyId = sellerCompanyId,
            SellerUserId = sellerUserId,
            OfferedAmount = total,
            Status = BidStatus.Open
        };
        await _db.Set<Bid>().AddAsync(bid, ct);
        await _db.SaveChangesAsync(ct);
        return Ok(new { bid.Id, bid.OfferedAmount, UnitPrice = request.UnitPrice, ap.Quantity });
    }

    // Bids for an auction product
    [HttpGet("{auctionId:int}/products/{auctionProductId:int}/bids")]
    public async Task<ActionResult<IEnumerable<object>>> GetBids(int auctionId, int auctionProductId, CancellationToken ct)
    {
        var bids = await _db.Set<Bid>()
            .Where(b => b.AuctionId == auctionId && b.AuctionProductId == auctionProductId)
            .OrderByDescending(b => b.CreatedOn)
            .Select(b => new { b.Id, b.SellerCompanyId, b.SellerUserId, b.OfferedAmount, b.Status, b.CreatedOn })
            .ToListAsync(ct);
        return Ok(bids);
    }

    // Full auction view: delivery address, products+variations, bids per product
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAuctionById(int id, CancellationToken ct)
    {
        var a = await _db.Set<Auction>()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Status,
                x.BuyerCompanyId,
                x.BuyerUserId,
                x.StartTime,
                x.EndTime,
                DeliveryAddress = x.DeliveryAddressId != null ? new
                {
                    x.DeliveryAddress!.FirstLine,
                    x.DeliveryAddress!.SecondLine,
                    x.DeliveryAddress!.City,
                    x.DeliveryAddress!.State,
                    x.DeliveryAddress!.PostalCode,
                    x.DeliveryAddress!.Longitude,
                    x.DeliveryAddress!.Latitude
                } : null,
                Products = _db.Set<AuctionProduct>()
                    .Where(ap => ap.AuctionId == x.Id)
                    .Select(ap => new
                    {
                        ap.Id,
                        ap.ProductId,
                        ap.ProductName,
                        ap.Quantity,
                        Variations = ap.Variations.Select(v => new { v.TypeId, v.Type, v.ValueId, v.Value }),
                        Bids = _db.Set<Bid>()
                            .Where(b => b.AuctionId == x.Id && b.AuctionProductId == ap.Id)
                            .OrderByDescending(b => b.CreatedOn)
                            .Select(b => new { b.Id, b.SellerCompanyId, b.SellerUserId, b.OfferedAmount, b.Status, b.CreatedOn })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
        if (a == null) return NotFound();
        return Ok(a);
    }

    // Buyer accepts a bid -> finalize auction product, create order + order product
    [HttpPost("{auctionId:int}/products/{auctionProductId:int}/accept/{bidId:int}")]
    public async Task<IActionResult> AcceptBid(int auctionId, int auctionProductId, int bidId, CancellationToken ct)
    {
        if (!TryGetUser(out var buyerUserId, out var buyerCompanyId)) return Unauthorized();
        var auction = await _db.Set<Auction>().FirstOrDefaultAsync(a => a.Id == auctionId, ct);
        if (auction == null) return NotFound("Auction not found");
        if (auction.BuyerCompanyId != buyerCompanyId || auction.BuyerUserId != buyerUserId)
            return Forbid();

        var ap = await _db.Set<AuctionProduct>().FirstOrDefaultAsync(x => x.Id == auctionProductId && x.AuctionId == auctionId, ct);
        if (ap == null) return NotFound("Auction product not found");

        var bid = await _db.Set<Bid>().FirstOrDefaultAsync(b => b.Id == bidId && b.AuctionId == auctionId && b.AuctionProductId == auctionProductId, ct);
        if (bid == null) return NotFound("Bid not found");
        if (bid.Status == BidStatus.Rejected) return BadRequest("Bid already rejected");

        // Accept selected bid; reject others
        var otherBids = await _db.Set<Bid>().Where(b => b.AuctionId == auctionId && b.AuctionProductId == auctionProductId && b.Id != bidId).ToListAsync(ct);
        foreach (var ob in otherBids) ob.Status = BidStatus.Rejected;
        bid.Status = BidStatus.Accepted;

        ap.AcceptedBidId = bid.Id;
        auction.Status = AuctionStatus.Completed;

        // Create order
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            BuyerCompanyId = buyerCompanyId,
            BuyerUserId = buyerUserId,
            SellerCompanyId = bid.SellerCompanyId,
            SellerUserId = bid.SellerUserId,
            OrderId = $"ORD-{auctionId}-{auctionProductId}-{bidId}",
            TotalAmount = bid.OfferedAmount,
            DeliveryAddressId = auction.DeliveryAddressId,
            Status = OrderStatus.Completed
        };
        await _db.Set<Order>().AddAsync(order, ct);
        await _db.SaveChangesAsync(ct);

        // Create order product (unit price inferred)
        var unitPrice = ap.Quantity == 0 ? bid.OfferedAmount : (bid.OfferedAmount / ap.Quantity);
        var op = new OrderProduct
        {
            OrderId = order.Id,
            ProductId = ap.ProductId,
            ProductName = ap.ProductName,
            Variations = ap.Variations, // copy selected variations to order item
            Quantity = ap.Quantity,
            UnitPrice = unitPrice,
            TotalPrice = bid.OfferedAmount
        };
        await _db.Set<OrderProduct>().AddAsync(op, ct);
        await _db.SaveChangesAsync(ct);

        return Ok(new { order.Id, order.OrderId, opId = op.Id });
    }

    // Buyer cancels auction
    [HttpPost("{auctionId:int}/cancel")]
    public async Task<IActionResult> Cancel(int auctionId, CancellationToken ct)
    {
        if (!TryGetUser(out var buyerUserId, out var buyerCompanyId)) return Unauthorized();
        var auction = await _db.Set<Auction>().FirstOrDefaultAsync(a => a.Id == auctionId, ct);
        if (auction == null) return NotFound();
        if (auction.BuyerCompanyId != buyerCompanyId || auction.BuyerUserId != buyerUserId) return Forbid();
        if (auction.Status != AuctionStatus.Open) return BadRequest("Auction not open");
        auction.Status = AuctionStatus.Cancelled;
        await _db.SaveChangesAsync(ct);
        return Ok();
    }
}
