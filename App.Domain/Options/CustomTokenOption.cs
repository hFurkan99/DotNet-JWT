namespace App.Domain.Options
{
    public class CustomTokenOption
    {
        public const string CustomToken = "CustomTokenOption";
        public List<string> Audience { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; } = default!;
    }
}
