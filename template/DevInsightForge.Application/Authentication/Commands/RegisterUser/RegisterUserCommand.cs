using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Interfaces.DataAccess;
using DevInsightForge.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using DevInsightForge.Application.Common.Services;
using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;

namespace DevInsightForge.Application.Authentication.Commands.RegisterUser;

public sealed record RegisterUserCommand : IRequest<TokenResponseModel>
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserModel> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenServices _tokenServices;
    private readonly JwtSettings _jwtSettings;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher<UserModel> passwordHasher,
        IOptions<JwtSettings> jwtSettings,
        IUnitOfWork unitOfWork,
        TokenServices tokenServices)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenServices = tokenServices;
        _jwtSettings = jwtSettings.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenResponseModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        UserModel user = UserModel.CreateUser(request.Email);
        user.SetPasswordHash(_passwordHasher.HashPassword(user, request.Password));

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var expiryDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);
        string accessToken = _tokenServices.GenerateJwtToken(user.Id, expiryDate);
        _tokenServices.SetAccessTokenCookie(accessToken, expiryDate);

        return new TokenResponseModel()
        {
            AccessToken = accessToken,
            AccessExpiresAt = expiryDate
        };
    }
}
