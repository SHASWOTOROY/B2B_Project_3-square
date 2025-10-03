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
public class ProductController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public ProductController(BaseDatabaseContext db)
    {
        _db = db;
    }

    // Input DTOs: frontend supplies type/value names only
    public record ProductVariationValueIn(string Name);
    public record ProductVariationTypeIn(string TypeName, List<ProductVariationValueIn> Values);
    public record CreateProductRequest(
        string Name,
        string ShortDescription,
        string Description,
        string Photo,
        decimal MrpPrice,
        int CategoryId,
        int BrandId,
        List<ProductVariationTypeIn>? Variations
    );

    private bool TryGetCompanyId(out int companyId)
    {
        companyId = 0;
        var cid = User.FindFirstValue("company_id") ?? User.FindFirstValue("companyId") ?? User.FindFirstValue("companyid");
        return int.TryParse(cid, out companyId);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        if (!TryGetCompanyId(out var companyId)) return Unauthorized();

        // Validate FK presence
        var categoryExists = await _db.Set<Category>().AnyAsync(c => c.Id == request.CategoryId, ct);
        if (!categoryExists) return BadRequest("Invalid CategoryId");
        var brandExists = await _db.Set<Brand>().AnyAsync(b => b.Id == request.BrandId, ct);
        if (!brandExists) return BadRequest("Invalid BrandId");

        // Build product
        var product = new Product
        {
            Name = request.Name,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            Photo = request.Photo,
            MRPPrice = request.MrpPrice,
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
            IsActive = true
        };

        // Resolve/create global variation types/values by name, then map to product-owned rows
        var productVarTypes = new List<ProductVariationType>();
        if (request.Variations != null)
        {
            foreach (var vt in request.Variations)
            {
                var typeName = vt.TypeName.Trim();
                // Find or create VariationType (global)
                var type = await _db.Set<VariationType>()
                    .FirstOrDefaultAsync(t => t.Name.ToLower() == typeName.ToLower(), ct);
                if (type == null)
                {
                    type = new VariationType { Name = typeName, IsActive = true };
                    await _db.Set<VariationType>().AddAsync(type, ct);
                    await _db.SaveChangesAsync(ct);
                }

                var pvValues = new List<ProductVariationValue>();
                foreach (var vv in vt.Values ?? new List<ProductVariationValueIn>())
                {
                    var valName = vv.Name.Trim();
                    var value = await _db.Set<VariationValue>()
                        .FirstOrDefaultAsync(x => x.VariationTypeId == type.Id && x.Name.ToLower() == valName.ToLower(), ct);
                    if (value == null)
                    {
                        value = new VariationValue { VariationTypeId = type.Id, Name = valName, IsActive = true };
                        await _db.Set<VariationValue>().AddAsync(value, ct);
                        await _db.SaveChangesAsync(ct);
                    }
                    pvValues.Add(new ProductVariationValue { ValueId = value.Id, Name = value.Name });
                }

                productVarTypes.Add(new ProductVariationType
                {
                    TypeId = type.Id,
                    Name = type.Name,
                    Values = pvValues
                });
            }
        }
        product.Variations = productVarTypes;

        await _db.Set<Product>().AddAsync(product, ct);
        await _db.SaveChangesAsync(ct);

        // Link to company in join table MARKETPLACE_ProductCompany
        // Since we modeled Many-to-Many, attach via navigation
        var company = await _db.Set<Company>().FindAsync([companyId], ct);
        if (company == null) return BadRequest("Company not found");

        product.Companies = product.Companies ?? new List<Company>();
        product.Companies.Add(company);
        await _db.SaveChangesAsync(ct);

        return Ok(new { product.Id, product.Name, product.CategoryId, product.BrandId });
    }

    // List all products (optional: filter by company)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] bool mine = false, CancellationToken ct = default)
    {
        IQueryable<Product> q = _db.Set<Product>().AsQueryable();
        if (mine && TryGetCompanyId(out var companyId))
        {
            q = q.Where(p => p.Companies!.Any(c => c.Id == companyId));
        }

        var list = await q
            .OrderBy(p => p.Name)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.CategoryId,
                CategoryName = p.Category!.Name,
                p.BrandId,
                BrandName = p.Brand!.Name,
                p.MRPPrice,
                Variations = p.Variations.Select(vt => new
                {
                    vt.TypeId,
                    vt.Name,
                    Values = vt.Values.Select(v => new { v.ValueId, v.Name })
                })
            })
            .ToListAsync(ct);
        return Ok(list);
    }

    // Get a single product with variations
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var p = await _db.Set<Product>()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.ShortDescription,
                x.Photo,
                x.MRPPrice,
                x.CategoryId,
                CategoryName = x.Category!.Name,
                x.BrandId,
                BrandName = x.Brand!.Name,
                Variations = x.Variations.Select(vt => new
                {
                    vt.TypeId,
                    vt.Name,
                    Values = vt.Values.Select(v => new { v.ValueId, v.Name })
                })
            })
            .FirstOrDefaultAsync(ct);
        if (p == null) return NotFound();
        return Ok(p);
    }
}
