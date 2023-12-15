namespace DevInsightForge.Application.Common.ViewModels.Authentication;

public class TokenUserModel
{
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
}
