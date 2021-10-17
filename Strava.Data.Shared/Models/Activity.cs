using Newtonsoft.Json;
using System;

namespace Strava.Data.Shared.Models
{
    public class Activity
    {
        [JsonProperty("resource_state")]
        public int ResourceState { get; set; }
        public object Athlete { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string DisplayDistance => $"{Math.Round(Distance / 1000.0), 2}km";

        [JsonProperty("moving_time")]
        public int MovingTime { get; set; }
        [JsonProperty("elaped_time")]
        public int ElapsedTime { get; set; }
        [JsonProperty("total_elevation_gain")]
        public double TotalElevationGain { get; set; }
        public string Type { get; set; }
        [JsonProperty("workout_type")]
        public object WorkoutType { get; set; }
        public long Id { get; set; }
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
        [JsonProperty("upload_id")]
        public string UploadId { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("start_date_local")]
        public DateTime StartDateLocal { get; set; }

        public string StartDateLocalFormatted => StartDateLocal.ToShortDateString();
        public string Timezone { get; set; }
        [JsonProperty("utc_offset")]
        public double UtcOffset { get; set; }
        [JsonProperty("start_latlng")]
        public double[] StartLatLng { get; set; }
        [JsonProperty("end_latlng")]
        public double[] EndLatLng { get; set; }
        [JsonProperty("location_city")]
        public string LocationCity { get; set; }
        [JsonProperty("location_state")]
        public string LocationState { get; set; }
        [JsonProperty("location_country")]
        public string LocationCountry { get; set; }
        [JsonProperty("start_latitude")]
        public double? StartLatitude { get; set; }
        [JsonProperty("start_longitude")]
        public double? StartLongitude { get; set; }
        [JsonProperty("achievement_count")]
        public int AchievementCount { get; set; }
        [JsonProperty("kudos_count")]
        public int KudosCount { get; set; }
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
        [JsonProperty("athlete_count")]
        public int AthleteCount { get; set; }
        [JsonProperty("photo_count")]
        public int PhotoCount { get; set; }
        public Map Map { get; set; }
        public bool Trainer { get; set; }
        public bool Commute { get; set; }
        public bool Manual { get; set; }
        public bool Private { get; set; }
        public string Visibility { get; set; }
        public bool Flagged { get; set; }
        [JsonProperty("gear_id")]
        public object GearId { get; set; }
        [JsonProperty("from_accepted_tag")]
        public bool FromAcceptedTag { get; set; }
        [JsonProperty("upload_id_str")]
        public string UploadIdStr { get; set; }
        [JsonProperty("average_speed")]
        public double AverageSpeed { get; set; }
        [JsonProperty("max_speed")]
        public double MaxSpeed { get; set; }
        [JsonProperty("average_cadence")]
        public double AverageCadence { get; set; }
        [JsonProperty("has_heartrate")]
        public bool HasHeartrate { get; set; }
        [JsonProperty("average_heartrate")]
        public double AverageHeartrate { get; set; }
        [JsonProperty("max_heartrate")]
        public double MaxHeartrate { get; set; }
        [JsonProperty("heartrate_opt_out")]
        public bool HeartrateOptOut { get; set; }
        [JsonProperty("display_hide_heartrate_option")]
        public bool DisplayHideHeartrateOption { get; set; }
        [JsonProperty("elev_high")]
        public double ElevationHigh { get; set; }
        [JsonProperty("elev_low")]
        public double ElevationLow { get; set; }
        [JsonProperty("pr_count")]
        public int PrCount { get; set; }
        [JsonProperty("total_photo_count")]
        public int TotalPhotoCount { get; set; }
        [JsonProperty("has_kudoed")]
        public bool HasKudoed { get; set; }

    }

    public class Map
    {
        public string Id { get; set; }
        [JsonProperty("summary_polyline")]
        public string SummaryPolyline { get; set; }
        public string Polyline { get; set; }
        [JsonProperty("resource_state")]
        public bool ResourceState { get; set; }
    }
}