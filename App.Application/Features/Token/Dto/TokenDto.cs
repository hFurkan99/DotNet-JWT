namespace App.Application.Features.Token.Dto
{
    public record TokenDto(
        string AccessToken,
        DateTime AccessTokenExpireDate,
        string RefreshToken,
        DateTime RefreshTokenExpireDate);
}
