namespace App.Domain.Options
{
    public class ClientOptions
    {
        public const string Clients = "ClientOptions";
        public string Id { get; set; } = default!;
        public string Secret { get; set; } = default!;
        public List<string>? Audiences { get; set; }
    }
}
