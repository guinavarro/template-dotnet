namespace Template.Domain.Models.Result;
public class AuthResult
{
    public AuthResult(
    string accessToken,
    string refreshToken,
    long expiresIn,
    DateTime expiresAt)
    {
        ExpiresIn = expiresIn;
        ExpiresAt = expiresAt;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
    public long ExpiresIn { get; protected set; }
    public DateTime ExpiresAt { get; protected set; }
    public string AccessToken { get; protected set; }
    public string RefreshToken { get; protected set; }
}

public record RefreshTokenResult
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public int ExpiresIn { get; set; }

    public DateTime ExpiresAt { get; set; }
}
