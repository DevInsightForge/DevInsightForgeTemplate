using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Interfaces.DataAccess;
using DevInsightForge.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;

namespace DevInsightForge.Application.Authentication.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand : IRequest<TokenResponseModel>
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}

internal sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, TokenResponseModel>
{
    private readonly IPasswordHasher<UserModel> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly TokenServices _tokenServices;
    private readonly JwtSettings _jwtSettings;

    public AuthenticateUserCommandHandler(
        IPasswordHasher<UserModel> passwordHasher,
        IOptions<JwtSettings> jwtSettings,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        TokenServices tokenServices)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenServices = tokenServices;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<TokenResponseModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        UserModel? user = await _userRepository.GetWhereAsync(u => u.NormalizedEmail == request.Email.ToUpperInvariant());

        if (user is null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) != PasswordVerificationResult.Success)
        {
            throw new BadRequestException("Invalid Credentials!");
        }

        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var expiryDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);
        string accessToken = _tokenServices.GenerateJwtToken(user, expiryDate);

        return new TokenResponseModel()
        {
            AccessToken = accessToken,
            AccessExpiresAt = expiryDate
        };
    }
}
