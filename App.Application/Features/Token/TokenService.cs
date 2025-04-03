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

namespace App.Application.Features.Token
{
    public class TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> tokenOption) : ITokenService
    {
        private readonly CustomTokenOption tokenOption = tokenOption.Value;

        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(tokenOption.RefreshTokenExpiration);

            var securityKey = SignHelper.GetSymmetricSecurityKey(tokenOption.SecurityKey);
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new(issuer: tokenOption.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(userApp, tokenOption.Audience),
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

        private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
        {
            var claimList = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, userApp.Email!),
                new(ClaimTypes.Name, userApp.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

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
