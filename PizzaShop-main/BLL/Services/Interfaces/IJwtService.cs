using System.Security.Claims;

namespace Pizza_Shop_Management_System.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string name, string email, string role);
    ClaimsPrincipal? ValidateToken(string token);
}
