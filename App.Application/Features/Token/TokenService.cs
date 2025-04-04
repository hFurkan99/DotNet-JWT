using App.Application.Features.Token.Dto;
using App.Domain.Entities;
using App.Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using App.Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using App.Application.Features.User.Dto;

namespace App.Application.Features.Token
{
    public class TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> tokenOption) : ITokenService
    {
        private readonly CustomTokenOption tokenOption = tokenOption.Value;

        public TokenDto CreateToken(UserAppDto dto)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(tokenOption.RefreshTokenExpiration);

            var securityKey = SignHelper.GetSymmetricSecurityKey(tokenOption.SecurityKey);
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new(issuer: tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(dto, tokenOption.Audience).Result,
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            TokenDto tokenDto = new(token, accessTokenExpiration, CreateRefreshToken(), refreshTokenExpiration);
            return tokenDto;
        }

        public ClientTokenDto CreateClientToken(ClientOptions clientOptions)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(tokenOption.AccessTokenExpiration);

            var securityKey = SignHelper.GetSymmetricSecurityKey(tokenOption.SecurityKey);
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new(issuer: tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClientClaims(clientOptions),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            ClientTokenDto tokenDto = new(token, accessTokenExpiration);
            return tokenDto;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        private async Task<IEnumerable<Claim>> GetClaims(UserAppDto dto, List<string> audiences)
        {
            var userApp = await userManager.FindByIdAsync(dto.Id.ToString()) ?? throw new HttpRequestException("User not found");

            var userRoles = await userManager.GetRolesAsync(userApp!);

            var claimList = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, dto.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, dto.Email!),
                new(ClaimTypes.Name, dto.Username!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            claimList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            return claimList;
        }

        private IEnumerable<Claim> GetClientClaims(ClientOptions clientOptions)
        {
            var claims = new List<Claim>();
            claims.AddRange(clientOptions!.Audiences!.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, clientOptions.Id.ToString()));


            return claims;
        }
    }
}
