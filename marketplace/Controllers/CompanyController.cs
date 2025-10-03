using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class CompanyController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public CompanyController(BaseDatabaseContext db)
    {
        _db = db;
    }

    public record CompanyDetailsRequest(string CompanyName, string Email, string Phone, string Website);
    public record CompanyAddressRequest(
        string CompanyName,
        string FirstLine,
        string SecondLine,
        string City,
        string State,
        string PostalCode,
        decimal Longitude,
        decimal Latitude
    );

    [HttpPost("details")]
    public async Task<IActionResult> UpsertDetails([FromBody] CompanyDetailsRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName)) return BadRequest("CompanyName is required");

        var company = await _db.Set<Company>().FirstOrDefaultAsync(c => c.Name == request.CompanyName, ct);
        if (company == null) return NotFound("Company not found");

        company.Email = request.Email ?? string.Empty;
        company.Phone = request.Phone ?? string.Empty;
        company.Website = request.Website ?? string.Empty;

        await _db.SaveChangesAsync(ct);
        return Ok(company);
    }

    [HttpPost("address")]
    public async Task<IActionResult> UpsertAddress([FromBody] CompanyAddressRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName)) return BadRequest("CompanyName is required");

        var company = await _db.Set<Company>().FirstOrDefaultAsync(c => c.Name == request.CompanyName, ct);
        if (company == null) return NotFound("Company not found");

        Address address;
        if (company.AddressId.HasValue)
        {
            address = await _db.Set<Address>().FirstAsync(a => a.Id == company.AddressId.Value, ct);
        }
        else
        {
            address = new Address();
            await _db.Set<Address>().AddAsync(address, ct);
        }

        address.FirstLine = request.FirstLine;
        address.SecondLine = request.SecondLine;
        address.City = request.City;
        address.State = request.State;
        address.PostalCode = request.PostalCode;
        address.Longitude = request.Longitude;
        address.Latitude = request.Latitude;

        await _db.SaveChangesAsync(ct);

        if (!company.AddressId.HasValue)
        {
            company.AddressId = address.Id;
            await _db.SaveChangesAsync(ct);
        }

        return Ok(new { company.Id, company.Name, company.AddressId });
    }
}
