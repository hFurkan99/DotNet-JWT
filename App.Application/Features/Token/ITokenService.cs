using App.Application.Features.Token.Dto;
using App.Domain.Entities;
using App.Domain.Options;

namespace App.Application.Features.Token
{
    public interface ITokenService
    {
        TokenDto CreateToken(UserApp userApp);
        ClientTokenDto CreateClientToken(ClientOptions clientOptions);
    }
}
