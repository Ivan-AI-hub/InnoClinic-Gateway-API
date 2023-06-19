namespace GatewayAPI.Web.Settings
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string IssuerSigningKey { get; set; }
        public double Expires { get; set; }
    }
}
