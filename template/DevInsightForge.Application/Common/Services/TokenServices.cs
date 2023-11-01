using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevInsightForge.Application.Common.Services;

public class TokenServices
{
    // Constants for claim types
    private const string ClaimTypeUserId = ClaimTypes.NameIdentifier;

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly JwtSettings _jwtSettings;

    public TokenServices(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        _jwtSettings = jwtSettings.Value;
    }

    public UserId GetLoggedInUserId()
    {
        var userIdClaim = (_contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypeUserId)) ?? throw new BadRequestException();
        if (!Ulid.TryParse(userIdClaim.Value?.Trim(), out var parsedUserId)) throw new BadRequestException();

        return new UserId(parsedUserId);
    }

    public string GenerateJwtToken(UserId userId)
    {
        if (userId is null) throw new ArgumentNullException(nameof(userId));

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypeUserId, userId.Value.ToString(), ClaimValueTypes.String),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var securityToken = new JwtSecurityToken(
          issuer: _jwtSettings.ValidIssuer,
          audience: _jwtSettings.ValidAudience,
          expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public void SetAccessTokenCookie(string token)
    {
        _contextAccessor?.HttpContext?.Response.Cookies.Append("Access-Token", token, new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes)
        });
    }

    public void RemoveAccessTokenCookie()
    {
        _contextAccessor?.HttpContext?.Response.Cookies.Append("Access-Token", "", new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.MinValue
        });
    }

}
