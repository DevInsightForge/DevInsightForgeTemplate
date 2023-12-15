using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
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

    private readonly IHttpContextAccessor _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public Guid GetLoggedInUserId()
    {
        var userIdClaim = (_contextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sid)) ?? throw new BadRequestException("User ID claim not found!");
        if (!Guid.TryParse(userIdClaim.Value?.Trim(), out var parsedUserId)) throw new BadRequestException("User ID claim is not valid!");

        return parsedUserId;
    }

    public string GenerateJwtToken(Guid userId, DateTime? expiryDate)
    {
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));
        expiryDate ??= DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sid, userId.ToString(), ClaimValueTypes.Sid),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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
