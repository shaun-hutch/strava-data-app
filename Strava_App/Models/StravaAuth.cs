using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strava_App.Models
{
    public class StravaAuth
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonIgnore]
        public DateTime ExpiryUtc => DateTimeOffset.FromUnixTimeSeconds(ExpiresAt).DateTime;
        [JsonIgnore]
        public DateTime ExpiryLocal => ExpiryUtc.ToLocalTime();
    }
}
