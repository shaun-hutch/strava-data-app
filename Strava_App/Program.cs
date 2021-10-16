using Strava.Data.Shared.Models;
using Strava.Data.Core.Helpers;
using Strava.Data.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strava.Data.Core
{
    class Program
    {
        // if for some reason we lose access for the app to access user account data, use this URL again
        // https://www.strava.com/oauth/authorize?client_id=68897&response_type=code&redirect_uri=http://localhost&approval_prompt=force&scope=activity:read_all,read

        // update this after going to the url above
        static readonly string code = "";

        static async Task Main(string[] args)
        {
            AppSettingsHelper.LoadAppSettings();

            // sets up http helper and sets the auth
            await HttpHelper.Init(AppSettingsHelper.Settings.ApiUrl, code);

            var activityService = new ActivityService();
            Activity[] activities = Array.Empty<Activity>();

            try
            {
                activities = await activityService.Get();
                Console.WriteLine($"there are {activities.Length} activities");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (activities?.Length > 0)
            {
                var first = await activityService.GetById(activities.First().Id);
                List<Location> points = PolylineHelper.DecodePolylinePoints(first.Map.SummaryPolyline);

                Console.Write($"there are {points.Count} location points from the activity");

            }
        }
    }
}