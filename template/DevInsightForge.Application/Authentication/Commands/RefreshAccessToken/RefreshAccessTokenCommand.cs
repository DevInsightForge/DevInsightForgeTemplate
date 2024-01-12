﻿using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Exceptions;
using DevInsightForge.Application.Common.Interfaces;
using DevInsightForge.Application.Common.ViewModels.Authentication;
using Microsoft.Extensions.Options;

namespace DevInsightForge.Application.Authentication.Commands.RefreshAccessToken;

public sealed record RefreshAccessTokenCommand : IRequest<TokenResponseModel>
{
    public string RefreshToken { get; set; } = string.Empty;
}

internal sealed class RefreshAccessTokenCommandHandler(
    IOptions<JwtSettings> jwtSettings,
    ITokenService tokenServices)
    : IRequestHandler<RefreshAccessTokenCommand, TokenResponseModel>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<TokenResponseModel> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = await tokenServices.GetValidRefreshToken(request.RefreshToken) ?? throw new BadRequestException("Refresh token provided is invalid or expired");

        var jwtExpiryDate = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes);
        string accessToken = tokenServices.GenerateJwtToken(userToken.UserId, jwtExpiryDate);

        return new TokenResponseModel()
        {
            RefreshToken = userToken.RefreshToken,
            RefreshTokenExpiresAt = userToken.ExpiresAt,
            AccessToken = accessToken,
            AccessTokenExpiresAt = jwtExpiryDate
        };
    }
}