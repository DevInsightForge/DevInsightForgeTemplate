using DevInsightForge.Domain.Entities.Core;

namespace DevInsightForge.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(Guid userId, DateTime? expiryDate);
    Task<string> GenerateRefreshTokenAsync(Guid userId, DateTime? expiryDate);
    Task<UserTokenModel?> GetValidRefreshToken(string refreshToken);
}
