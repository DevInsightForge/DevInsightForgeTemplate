using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Domain.Entities.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevInsightForge.Application.Common.Services;

public class TokenServices(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor contextAccessor)
{
    // Custom Claims
    private const string UserIdClaim = ClaimTypes.NameIdentifier;

    private readonly IHttpContextAccessor _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public Guid GetLoggedInUserId()
    {
        var userIdClaim = (_contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)) ?? throw new BadRequestException("User ID claim not found!");
        if (!Guid.TryParse(userIdClaim.Value?.Trim(), out var parsedUserId)) throw new BadRequestException("User ID claim is not valid!");

        return parsedUserId;
    }

    public string GenerateJwtToken(UserModel user, DateTime? expiryDate)
    {
        ArgumentNullException.ThrowIfNull(user);
        expiryDate ??= DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

        var authClaims = new List<Claim>
        {
            new(UserIdClaim, user.Id.ToString(), ClaimValueTypes.Sid),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var securityToken = new JwtSecurityToken(
          issuer: _jwtSettings.ValidIssuer,
          audience: _jwtSettings.ValidAudience,
          expires: expiryDate,
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
