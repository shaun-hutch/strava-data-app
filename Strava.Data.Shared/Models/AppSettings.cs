namespace Strava.Data.Shared.Models
{
    public class AppSettings
    {
        public int ClientId { get; set; }
        public string ClientSecret { get; set; } 
        public string ApiUrl { get; set; }
        public string AuthJsonFile { get; set; }
    }
}
