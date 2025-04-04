using App.Application.Features.Token.Dto;
using App.Application.Features.User.Dto;
using App.Domain.Entities;
using App.Domain.Options;

namespace App.Application.Features.Token
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserAppDto dto);
        ClientTokenDto CreateClientToken(ClientOptions clientOptions);
    }
}
