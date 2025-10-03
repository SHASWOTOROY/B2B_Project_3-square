using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly BaseDatabaseContext _db;

    public UsersController(BaseDatabaseContext db)
    {
        _db = db;
    }

    public record UpdateUserRequest(string Name, string Email, string Phone, string? Username);

    private bool TryGetUserContext(out int userId, out int companyId)
    {
        userId = 0; companyId = 0;
        // Support both 'sub' and NameIdentifier
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var companyIdClaim = User.FindFirstValue("company_id") ?? User.FindFirstValue("companyId") ?? User.FindFirstValue("companyid");
        return int.TryParse(sub, out userId) && int.TryParse(companyIdClaim, out companyId);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        if (!TryGetUserContext(out var userId, out var companyId)) return Unauthorized();

        var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == companyId, ct);
        if (user == null) return NotFound("User not found");

        return Ok(new
        {
            user.Id,
            user.CompanyId,
            user.Username,
            user.Name,
            user.Email,
            user.Phone
        });
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequest request, CancellationToken ct)
    {
        if (!TryGetUserContext(out var userId, out var companyId)) return Unauthorized();

        var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == companyId, ct);
        if (user == null) return NotFound("User not found");

        // Update fields
        user.Name = request.Name ?? string.Empty;
        user.Email = request.Email ?? string.Empty;
        user.Phone = request.Phone ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(request.Username) && !string.Equals(request.Username, user.Username, StringComparison.Ordinal))
        {
            var exists = await _db.Set<User>().AnyAsync(u => u.CompanyId == companyId && u.Username == request.Username, ct);
            if (exists) return Conflict("Username already exists in this company");
            user.Username = request.Username!;
        }

        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            user.Id,
            user.CompanyId,
            user.Username,
            user.Name,
            user.Email,
            user.Phone
        });
    }
}
