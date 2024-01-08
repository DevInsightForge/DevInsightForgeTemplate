namespace DevInsightForge.Application.Authentication.Commands.RefreshAccessToken;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(command => command.RefreshToken)
            .NotEmpty().WithMessage("Refresh Token is required.");
    }
}
