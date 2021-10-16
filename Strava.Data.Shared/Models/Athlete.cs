using Newtonsoft.Json;
using System;

namespace Strava.Data.Shared.Models
{
    public class Athlete
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ResourceState { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Sex { get; set; }
        public bool Premium { get; set; }
        public bool Summit { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("badge_type_id")]
        public int BadgeTypeId { get; set; }
        public float Weight { get; set; }
        [JsonProperty("profile_medium")]
        public string ProfileMedium { get; set; }
        public string Profile { get; set; }
        public object Friend { get; set; }
        public object Follower { get; set; }

        public string Name => $"{Firstname} {Lastname}";
    }
}