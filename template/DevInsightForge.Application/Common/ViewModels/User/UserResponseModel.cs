namespace DevInsightForge.Application.Common.ViewModels.User;

public class UserResponseModel
{
    public long UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime DateJoined { get; set; }
    public DateTime LastLogin { get; set; }
}
