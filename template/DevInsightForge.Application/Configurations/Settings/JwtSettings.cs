namespace DevInsightForge.Application.Configurations.Settings;

public class JwtSettings
{
    public string SecretKey { get; set; } = "Default_Super_Secret_256_Bits_Signing_Key";
    public string ValidIssuer { get; set; } = "DefaultIssuer";
    public string ValidAudience { get; set; } = "DefaultAudience";
    public double AccessTokenExpirationInMinutes { get; set; } = 60;
    public double RefreshTokenExpirationInMinutes { get; set; } = 1440;
}
