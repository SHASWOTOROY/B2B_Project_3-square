namespace MarketPlace.Api.Dtos;

public record SignupRequest(
    string CompanyName,
    string Name,
    string Email,
    string Phone,
    string Username,
    string Password
);

public record LoginRequest(
    string Username,
    string Password,
    string CompanyName
);

public record AuthResponse(
    string Token,
    DateTime ExpiresAt,
    int UserId,
    int CompanyId,
    string Username,
    string Name,
    string Email
);
