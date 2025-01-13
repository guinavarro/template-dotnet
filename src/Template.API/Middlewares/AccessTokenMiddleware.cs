using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Template.Domain.Interfaces.Services;

namespace Template.API.Middlewares;
public class AccessTokenMiddleware
{

    private readonly RequestDelegate _next;

    public AccessTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var authorizeAttribute = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>();

        if (authorizeAttribute == null)
        {
            await _next(context);
            return;
        }

        var bearerToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var refreshToken = context.Request.Headers["Refresh-Token"].ToString();

        if (string.IsNullOrEmpty(bearerToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Acess Token is missing");
            return;
        }

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(bearerToken))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Access Token");
                return;
            }

            var jwtToken = jwtHandler.ReadJwtToken(bearerToken);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");

            if (expClaim != null && long.TryParse(expClaim.Value, out var exp))
            {
                var expiration = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
                if (expiration < DateTime.UtcNow)
                {
                    if (string.IsNullOrEmpty(refreshToken))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Refresh Token is missing");
                        return;
                    }

                    try
                    {
                        var _authService = context.RequestServices.GetRequiredService<IAuthService>();
                        var newTokens = await _authService.RefreshAccessTokenAsync(refreshToken);
                        context.Response.Headers.Append("New-Access-Token", newTokens.AccessToken);
                        context.Response.Headers.Append("New-Refresh-Token", newTokens.RefreshToken);
                    }
                    catch
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid or expired Refresh Token");
                        return;
                    }
                }
            }

            await _next(context);
        }
        catch
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occurred while validating the Access Token");
        }
    }
}
