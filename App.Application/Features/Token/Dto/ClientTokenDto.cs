namespace App.Application.Features.Token.Dto
{
    public record ClientTokenDto(string AccessToken, DateTime AccessTokenExpireDate);
}
