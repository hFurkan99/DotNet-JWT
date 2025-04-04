using App.Application.Features.Authentication;
using App.Application.Features.Authentication.Login;
using App.Application.Features.Token.Dto;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    public class AuthController(IAuthenticationService authenticationService) : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginRequest request)
        {
            return CreateActionResult(await authenticationService.CreateTokenAsync(request));
        }

        [HttpPost]
        public IActionResult CreateClientToken(ClientLoginRequest request)
        {
            return CreateActionResult(authenticationService.CreateClientTokenAsync(request));
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto request)
        {
            return CreateActionResult( await authenticationService.RevokeRefreshTokenAsync(request.RefreshToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto request)
        {
            return CreateActionResult(await authenticationService.CreateTokenByRefreshTokenAsync(request.RefreshToken));
        }

    }
}
