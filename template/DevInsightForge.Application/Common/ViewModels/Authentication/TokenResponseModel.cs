using System.Text.Json.Serialization;

namespace DevInsightForge.Application.Common.ViewModels.Authentication;

public class TokenResponseModel
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; set; } = null;
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessExpiresAt { get; set; }
}
