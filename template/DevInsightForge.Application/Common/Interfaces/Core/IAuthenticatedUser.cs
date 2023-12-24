namespace DevInsightForge.Application.Common.Interfaces.Core;

public interface IAuthenticatedUser
{
    Guid? UserId { get; }
}
