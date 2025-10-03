using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using MarketPlace.Api.Dtos;
using MarketPlace.Models;
using MarketPlace.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RepositorySession _session;
    private readonly BaseDatabaseContext _db;
    private readonly IConfiguration _config;

    public AuthController(RepositorySession session, BaseDatabaseContext db, IConfiguration config)
    {
        _session = session;
        _db = db;
        _config = config;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> Signup([FromBody] SignupRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName) || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("CompanyName, Username and Password are required");

        // Resolve or create company by name (only name at signup)
        var company = await _db.Set<Company>().FirstOrDefaultAsync(c => c.Name == request.CompanyName, ct);
        if (company == null)
        {
            company = new Company
            {
                Name = request.CompanyName,
                Email = string.Empty,
                Phone = string.Empty,
                Website = string.Empty,
                IsActive = true
            };
            await _db.Set<Company>().AddAsync(company, ct);
            await _db.SaveChangesAsync(ct);
        }

        // Check username uniqueness within the company
        var existingUser = await _db.Set<User>().FirstOrDefaultAsync(u => u.Username == request.Username && u.CompanyId == company.Id, ct);
        if (existingUser != null)
            return Conflict("Username already exists in this company");

        // Create user with hashed password
        var hashed = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            CompanyId = company.Id,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Username = request.Username,
            Password = hashed,
            IsActive = true
        };
        await _db.Set<User>().AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);

        var token = GenerateJwt(user);
        return Ok(new AuthResponse(
            token.Token,
            token.ExpiresAt,
            user.Id,
            user.CompanyId,
            user.Username,
            user.Name,
            user.Email
        ));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName))
            return BadRequest("CompanyName is required");

        var company = await _db.Set<Company>().FirstOrDefaultAsync(c => c.Name == request.CompanyName, ct);
        if (company == null)
            return Unauthorized("Invalid company");

        var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Username == request.Username && u.CompanyId == company.Id && u.IsActive, ct);
        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized("Invalid credentials");

        var token = GenerateJwt(user);
        return Ok(new AuthResponse(
            token.Token,
            token.ExpiresAt,
            user.Id,
            user.CompanyId,
            user.Username,
            user.Name,
            user.Email
        ));
    }

    private (string Token, DateTime ExpiresAt) GenerateJwt(User user)
    {
        var key = _config["Jwt:Key"] ?? "ChangeThisDevelopmentKeyToAStrongRandomValue";
        var issuer = _config["Jwt:Issuer"] ?? "MarketPlaceIssuer";
        var audience = _config["Jwt:Audience"] ?? "MarketPlaceAudience";
        var expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
            new("company_id", user.CompanyId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresAt);
    }
}
