using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using Template.Domain.Interfaces.Services;
using Template.Domain.Models.Command;
using Template.Domain.Models.Result;

namespace Template.Domain.Services;

public class AuthService : IAuthService
{
    private readonly SupabaseService _supabaseService;

    public async Task<AuthResult> Authenticate(AuthCommand command, CancellationToken ct = default)
    {
        var result = await _supabaseService.GetClient().Auth.SignIn(command.Email, command.Password);
        return new AuthResult(result.AccessToken, result.RefreshToken, result.ExpiresIn, result.ExpiresAt());
    }

    public async Task<RefreshTokenResult> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        try
        {
            var options = new RestClientOptions(""); // SupabasesEndpoint
            var client = new RestClient(options);

            var url = "token?grant_type=refresh_token"; 

            var body = new
            {
                refresh_token = refreshToken
            };

            var request = new RestRequest(url, Method.Post).AddJsonBody(body);
            request.AddHeader("apiKey", "");
            var response = await client.ExecuteAsync(request, ct);

            var json = JObject.Parse(response.Content);

            var result = new RefreshTokenResult
            {
                AccessToken = json["access_token"].ToString(),
                RefreshToken = json["refresh_token"].ToString(),
                ExpiresIn = Convert.ToInt32(json["expires_in"]),
                ExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToDouble(json["expires_in"]))
            };
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<AuthResult> RegisterUser(AuthCommand command, CancellationToken ct = default)
    {
        var result = await _supabaseService.GetClient().Auth.SignUp(command.Email, command.Password);
        return new AuthResult(result.AccessToken, result.RefreshToken, result.ExpiresIn, result.ExpiresAt());
    }
}

