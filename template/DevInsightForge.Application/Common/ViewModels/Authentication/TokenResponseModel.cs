using System.Text.Json.Serialization;

namespace DevInsightForge.Application.Common.ViewModels.Authentication;

public class TokenResponseModel
{
    public string AccessToken { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; set; } = null;
}
