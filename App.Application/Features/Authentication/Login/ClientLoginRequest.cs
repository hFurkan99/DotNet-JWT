namespace App.Application.Features.Authentication.Login
{
    public record ClientLoginRequest(string ClientId, string ClientSecret);
}