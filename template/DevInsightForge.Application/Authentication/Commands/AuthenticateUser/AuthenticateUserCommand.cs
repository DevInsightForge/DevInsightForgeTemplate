using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Application.Common.Interfaces.DataAccess;
using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;
using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using DevInsightForge.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevInsightForge.Application.Authentication.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand : IRequest<TokenResponseModel>
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}

internal sealed class AuthenticateUserCommandHandler(
    IPasswordHasher<UserModel> passwordHasher,
    IOptions<JwtSettings> jwtSettings,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    TokenServices tokenServices) : IRequestHandler<AuthenticateUserCommand, TokenResponseModel>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<TokenResponseModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        UserModel? user = await userRepository.GetWhereAsync(u => u.NormalizedEmail.Equals(request.Email));

        if (user is null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
        {
            throw new BadRequestException("Invalid Credentials!");
        }

        user.UpdateLastLogin();
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var expiryDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);
        string accessToken = tokenServices.GenerateJwtToken(user, expiryDate);

        return new TokenResponseModel()
        {
            AccessToken = accessToken,
            AccessExpiresAt = expiryDate
        };
    }
}
