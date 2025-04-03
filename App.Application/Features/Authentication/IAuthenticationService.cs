using App.Application.Features.Authentication.Login;
using App.Application.Features.Token.Dto;

namespace App.Application.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<TokenDto>> CreateTokenAsync(LoginRequest request);
        Task<ServiceResult<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
        Task<ServiceResult> RevokeRefreshTokenAsync(string refreshToken);
        ServiceResult<ClientTokenDto> CreateClientTokenAsync(ClientLoginRequest request);
    }
}
