using Template.Domain.Models.Command;
using Template.Domain.Models.Result;

namespace Template.Domain.Interfaces.Services;

public interface IAuthService
    {
        Task<RefreshTokenResult> RefreshAccessTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<AuthResult> Authenticate(AuthCommand command, CancellationToken ct = default);
        Task<AuthResult> RegisterUser(AuthCommand command, CancellationToken ct = default);
    }

