using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class CategoryController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public CategoryController(BaseDatabaseContext db)
    {
        _db = db;
    }

    public record CreateCategoryRequest(string Name);

    // Create a root category. After insert, set ParentCategoryId = Id as per your rule.
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest("Name is required");

        var normalized = request.Name.Trim();
        var exists = await _db.Set<Category>().AnyAsync(c => c.Name.ToLower() == normalized.ToLower(), ct);
        if (exists) return Conflict("Category with this name already exists (case-insensitive)");

        var category = new Category
        {
            Name = normalized,
            IsActive = true
        };
        await _db.Set<Category>().AddAsync(category, ct);
        await _db.SaveChangesAsync(ct);

        // Set ParentCategoryId to its own Id
        category.ParentCategoryId = category.Id;
        await _db.SaveChangesAsync(ct);

        return Ok(new { category.Id, category.Name, category.ParentCategoryId });
    }

    // Get all categories (for dropdown)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll(CancellationToken ct)
    {
        var items = await _db.Set<Category>()
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name, c.ParentCategoryId })
            .ToListAsync(ct);
        return Ok(items);
    }
}
