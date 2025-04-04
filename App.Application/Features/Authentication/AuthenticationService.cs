using App.Application.Contracts.Persistence;
using App.Application.Features.Authentication.Login;
using App.Application.Features.Token;
using App.Application.Features.Token.Dto;
using App.Application.Features.User.Dto;
using App.Domain.Entities;
using App.Domain.Options;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;

namespace App.Application.Features.Authentication
{
    internal class AuthenticationService(
        UserManager<UserApp> userManager,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IGenericRepository<UserRefreshToken, long> userRefreshTokenRepository,
        IOptions<List<ClientOptions>> clientOptions,
        IMapper mapper) : IAuthenticationService
    {
        private readonly List<ClientOptions> clientOptions = clientOptions.Value;

        public async Task<ServiceResult<TokenDto>> CreateTokenAsync(LoginRequest request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));

            var user = await userManager.FindByEmailAsync(request.Email);

            if(user == null) 
                return ServiceResult<TokenDto>.Fail("Email or password is wrong.");

            if (!await userManager.CheckPasswordAsync(user, request.Password))
                return ServiceResult<TokenDto>.Fail("Email or password is wrong.");

            var userDto = mapper.Map<UserAppDto>(user);

            var token = tokenService.CreateToken(userDto);

            var userRefreshToken = await userRefreshTokenRepository.FirstOrDefaultAsync(x => x.UserId == user.Id);
                
            if (userRefreshToken == null)
            {
                UserRefreshToken newUserRefreshToken = new()
                {
                    UserId = user.Id,
                    Code = token.RefreshToken, 
                    ExpireDate = token.RefreshTokenExpireDate
                };

                await userRefreshTokenRepository.AddAsync(newUserRefreshToken);
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.ExpireDate = token.RefreshTokenExpireDate;
                userRefreshTokenRepository.Update(userRefreshToken);
            }

            await unitOfWork.SaveChangesAsync();
            return ServiceResult<TokenDto>.Success(token);
        }

        public ServiceResult<ClientTokenDto> CreateClientTokenAsync(ClientLoginRequest request)
        {
            var client = clientOptions.SingleOrDefault(x => x.Id == request.ClientId && x.Secret == request.ClientSecret);

            if (client == null)
                return ServiceResult<ClientTokenDto>.Fail("ClientId or ClientSecret not found.", HttpStatusCode.NotFound);

            var token = tokenService.CreateClientToken(client);
            return ServiceResult<ClientTokenDto>.Success(token);
        }

        public async Task<ServiceResult<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await userRefreshTokenRepository.FirstOrDefaultAsync(x => x.Code == refreshToken);

            if (existRefreshToken == null)
                return ServiceResult<TokenDto>.Fail("Refresh token not found.", HttpStatusCode.NotFound);

            var user = await userManager.FindByIdAsync(existRefreshToken.UserId.ToString());

            if (user == null)
                return ServiceResult<TokenDto>.Fail("User not found.", HttpStatusCode.NotFound);

            var userDto = mapper.Map<UserAppDto>(user);

            var token = tokenService.CreateToken(userDto);

            existRefreshToken.Code = token.RefreshToken;
            existRefreshToken.ExpireDate = token.RefreshTokenExpireDate;
            
            userRefreshTokenRepository.Update(existRefreshToken);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<TokenDto>.Success(token);
        }

        public async Task<ServiceResult> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await userRefreshTokenRepository.FirstOrDefaultAsync(x => x.Code == refreshToken);

            if (existRefreshToken == null)
                return ServiceResult.Fail("Refresh token not found.", HttpStatusCode.NotFound);

            userRefreshTokenRepository.Delete(existRefreshToken);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.OK);
        }
    }
}
