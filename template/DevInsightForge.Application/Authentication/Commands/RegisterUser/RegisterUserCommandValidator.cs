using DevInsightForge.Application.Authentication.Commands.RegisterUser;
using DevInsightForge.Application.Common.Interfaces.DataAccess.Repositories;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(BeUniqueEmail).WithMessage("Email is already registered.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).+$")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _userRepository.AnyAsync(u => u.Email == email);
    }
}
