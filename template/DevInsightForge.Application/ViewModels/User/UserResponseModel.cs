namespace DevInsightForge.Application.ViewModels.User;

public class UserResponseModel
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset DateJoined { get; set; }
    public DateTimeOffset LastLogin { get; set; }
}
