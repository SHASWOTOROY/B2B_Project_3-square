using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/variation")]
[Authorize]
public class VariationController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public VariationController(BaseDatabaseContext db)
    {
        _db = db;
    }

    public record CreateTypeRequest(string Name);
    public record CreateValueRequest(string Name);

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<object>>> GetTypes(CancellationToken ct)
    {
        var items = await _db.Set<VariationType>()
            .OrderBy(t => t.Name)
            .Select(t => new { t.Id, t.Name })
            .ToListAsync(ct);
        return Ok(items);
    }

    [HttpPost("types")]
    public async Task<IActionResult> CreateType([FromBody] CreateTypeRequest request, CancellationToken ct)
    {
        var name = request.Name.Trim();
        var exists = await _db.Set<VariationType>().AnyAsync(t => t.Name.ToLower() == name.ToLower(), ct);
        if (exists) return Conflict("Variation type already exists (case-insensitive)");
        var type = new VariationType { Name = name, IsActive = true };
        await _db.Set<VariationType>().AddAsync(type, ct);
        await _db.SaveChangesAsync(ct);
        return Ok(new { type.Id, type.Name });
    }

    [HttpGet("types/{typeId:int}/values")]
    public async Task<ActionResult<IEnumerable<object>>> GetValues(int typeId, CancellationToken ct)
    {
        var items = await _db.Set<VariationValue>()
            .Where(v => v.VariationTypeId == typeId)
            .OrderBy(v => v.Name)
            .Select(v => new { v.Id, v.Name })
            .ToListAsync(ct);
        return Ok(items);
    }

    [HttpPost("types/{typeId:int}/values")]
    public async Task<IActionResult> CreateValue(int typeId, [FromBody] CreateValueRequest request, CancellationToken ct)
    {
        var name = request.Name.Trim();
        var typeExists = await _db.Set<VariationType>().AnyAsync(t => t.Id == typeId, ct);
        if (!typeExists) return BadRequest("Variation type not found");
        var exists = await _db.Set<VariationValue>().AnyAsync(v => v.VariationTypeId == typeId && v.Name.ToLower() == name.ToLower(), ct);
        if (exists) return Conflict("Variation value already exists under this type (case-insensitive)");
        var value = new VariationValue { VariationTypeId = typeId, Name = name, IsActive = true };
        await _db.Set<VariationValue>().AddAsync(value, ct);
        await _db.SaveChangesAsync(ct);
        return Ok(new { value.Id, value.Name });
    }
}
