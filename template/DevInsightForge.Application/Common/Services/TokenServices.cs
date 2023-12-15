using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Application.Common.ViewModels.User;
using DevInsightForge.Domain.Entities.User;
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
    private const string EmailClaim = ClaimTypes.Email;
    private const string DateJoinedClaim = "dj";
    private const string LastLoginClaim = "ll";

    private readonly IHttpContextAccessor _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public long GetLoggedInUserId()
    {
        var userIdClaim = (_contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)) ?? throw new BadRequestException("User ID claim not found!");
        if (!long.TryParse(userIdClaim.Value?.Trim(), out var parsedUserId)) throw new BadRequestException("User ID claim is not valid!");

        return parsedUserId;
    }

    public UserResponseModel GetLoggedInUser()
    {
        ClaimsPrincipal principal = _contextAccessor?.HttpContext?.User ?? throw new BadRequestException("User claims are not found!");
        string userTokenId = principal.FindFirstValue(UserIdClaim) ?? throw new BadRequestException("User ID claim not found!");
        string userEmail = principal.FindFirstValue(EmailClaim) ?? throw new BadRequestException("User Email claim not found!");
        string userDateJoined = principal.FindFirstValue(DateJoinedClaim) ?? throw new BadRequestException("User DateJoined claim not found!");
        string userLastLogin = principal.FindFirstValue(LastLoginClaim) ?? throw new BadRequestException("User LastLogin claim not found!");

        long userId = long.TryParse(userTokenId, out var parsedUserId) ? parsedUserId : throw new BadRequestException("User ID claim is not valid!");
        DateTime dateJoined = DateTime.TryParse(userDateJoined, out var dj) ? dj : throw new BadRequestException("User DateJoined claim is not a valid DateTime");
        DateTime lastLogin = DateTime.TryParse(userLastLogin, out var ll) ? ll : throw new BadRequestException("User LastLogin claim is not a valid DateTime");


        return new UserResponseModel
        {
            UserId = userId,
            Email = userEmail,
            DateJoined = dateJoined,
            LastLogin = lastLogin
        };
    }

    public string GenerateJwtToken(UserModel user, DateTime? expiryDate)
    {
        ArgumentNullException.ThrowIfNull(user);
        expiryDate ??= DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);

        var authClaims = new List<Claim>
        {
            new(UserIdClaim, user.Id.ToString(), ClaimValueTypes.Sid),
            new(EmailClaim, user.Email, ClaimValueTypes.Email),
            new(DateJoinedClaim, user.DateJoined.ToString(), ClaimValueTypes.DateTime),
            new(LastLoginClaim, user.LastLogin.ToString(), ClaimValueTypes.DateTime),
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
