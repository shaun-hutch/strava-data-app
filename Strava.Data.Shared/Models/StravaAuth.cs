using Newtonsoft.Json;
using System;

namespace Strava.Data.Shared.Models
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

    public class StravaAuthRequest
    {
        public string Code { get; set; }
        public int UserId { get; set; }
    }
}
