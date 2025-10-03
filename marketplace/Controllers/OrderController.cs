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
public class OrderController : ControllerBase
{
    private readonly BaseDatabaseContext _db;
    public OrderController(BaseDatabaseContext db) { _db = db; }

    private bool TryGetCompany(out int companyId)
    {
        companyId = 0;
        var cid = User.FindFirstValue("company_id") ?? User.FindFirstValue("companyId") ?? User.FindFirstValue("companyid");
        return int.TryParse(cid, out companyId);
    }

    // List orders for the current user's company as buyer or seller
    // as=buyer|seller|all (default: all)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] string? asRole, CancellationToken ct)
    {
        if (!TryGetCompany(out var companyId)) return Unauthorized();
        IQueryable<Order> q = _db.Set<Order>();
        asRole = (asRole ?? "all").ToLowerInvariant();
        q = asRole switch
        {
            "buyer" => q.Where(o => o.BuyerCompanyId == companyId),
            "seller" => q.Where(o => o.SellerCompanyId == companyId),
            _ => q.Where(o => o.BuyerCompanyId == companyId || o.SellerCompanyId == companyId)
        };

        var list = await q
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new
            {
                o.Id,
                o.OrderId,
                o.OrderDate,
                o.BuyerCompanyId,
                o.SellerCompanyId,
                o.TotalAmount,
                o.Status,
                DeliveryAddress = o.DeliveryAddressId != null ? new
                {
                    o.DeliveryAddress!.FirstLine,
                    o.DeliveryAddress!.SecondLine,
                    o.DeliveryAddress!.City,
                    o.DeliveryAddress!.State,
                    o.DeliveryAddress!.PostalCode,
                    o.DeliveryAddress!.Longitude,
                    o.DeliveryAddress!.Latitude
                } : null,
                Products = _db.Set<OrderProduct>()
                    .Where(op => op.OrderId == o.Id)
                    .Select(op => new
                    {
                        op.Id,
                        op.ProductId,
                        op.ProductName,
                        op.Quantity,
                        op.UnitPrice,
                        op.TotalPrice,
                        Variations = op.Variations.Select(v => new { v.TypeId, v.Type, v.ValueId, v.Value })
                    })
                    .ToList()
            })
            .ToListAsync(ct);

        return Ok(list);
    }

    // Get a single order (buyer or seller from this company)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        if (!TryGetCompany(out var companyId)) return Unauthorized();
        var o = await _db.Set<Order>()
            .Where(x => x.Id == id && (x.BuyerCompanyId == companyId || x.SellerCompanyId == companyId))
            .Select(x => new
            {
                x.Id,
                x.OrderId,
                x.OrderDate,
                x.BuyerCompanyId,
                x.SellerCompanyId,
                x.BuyerUserId,
                x.SellerUserId,
                x.TotalAmount,
                x.Status,
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
                Products = _db.Set<OrderProduct>()
                    .Where(op => op.OrderId == x.Id)
                    .Select(op => new
                    {
                        op.Id,
                        op.ProductId,
                        op.ProductName,
                        op.Quantity,
                        op.UnitPrice,
                        op.TotalPrice,
                        Variations = op.Variations.Select(v => new { v.TypeId, v.Type, v.ValueId, v.Value })
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
        if (o == null) return NotFound();
        return Ok(o);
    }
}
