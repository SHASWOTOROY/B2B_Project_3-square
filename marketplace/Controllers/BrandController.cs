using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BrandController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public BrandController(BaseDatabaseContext db)
    {
        _db = db;
    }

    public record CreateBrandRequest(string Name);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Name is required");

        var normalized = request.Name.Trim();
        var exists = await _db.Set<Brand>().AnyAsync(b => b.Name.ToLower() == normalized.ToLower(), ct);
        if (exists) return Conflict("Brand with this name already exists (case-insensitive)");

        var brand = new Brand
        {
            Name = request.Name.Trim(),
            IsActive = true
        };
        await _db.Set<Brand>().AddAsync(brand, ct);
        await _db.SaveChangesAsync(ct);

        return Ok(new { brand.Id, brand.Name });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll(CancellationToken ct)
    {
        var items = await _db.Set<Brand>()
            .OrderBy(b => b.Name)
            .Select(b => new { b.Id, b.Name })
            .ToListAsync(ct);
        return Ok(items);
    }
}
