using Newtonsoft.Json;
using Strava_App.Helpers;
using Strava_App.Models;
using Strava_App.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Strava_App
{
    class Program
    {
        // if for some reason we lose access for the app to access user account data, use this URL again
        // https://www.strava.com/oauth/authorize?client_id=68897&response_type=code&redirect_uri=http://localhost&approval_prompt=force&scope=activity:read_all,read

        // update this after going to the url above
        static string code = "";
        static StravaAuth Auth;

        static async Task Main(string[] args)
        {
            AppSettingsHelper.LoadAppSettings();
            HttpHelper.Init(AppSettingsHelper.Settings.ApiUrl);
            var service = new AuthService();

            Auth = await service.LoadAuth();

            if (Auth == null)
            {
                Console.WriteLine("unable to acquire auth from JSON file.");

                if (!string.IsNullOrEmpty(code))
                {
                    Auth = await service.GetAccessToken(code);
                    await service.WriteAuth(Auth);
                }
                else
                {
                    Console.WriteLine("unable to get initial access token");
                    Environment.Exit(0);
                }
            }

            // refresh token if need be
            if (DateTime.UtcNow.Ticks > Auth.ExpiryUtc.Ticks)
            {
                Auth = await service.RefreshToken(Auth.RefreshToken);
                if (!await service.WriteAuth(Auth))
                {
                    Console.WriteLine("unable to write auth to JSON file.");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"Auth JSON written to {AppSettingsHelper.Settings.AuthJsonFile}");
                }
            }

            HttpHelper.SetAuth(Auth.AccessToken);
            Activity[] activities = new Activity[] { };

            try
            {
                var result = await HttpHelper.Get<Activity[]>("activities");
                activities = result.Result;
                Console.WriteLine($"there are {activities.Length} activities");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("yee");

            if (activities.Length > 0)
            {
                var first = activities.First();
                var apiFirst = await HttpHelper.Get<Activity>($"activities/{first.Id}");
                
                List<Location> points = PolylineHelper.DecodePolylinePoints(apiFirst.Result.Map.SummaryPolyline);

                Console.Write($"there are {points.Count} location points from the activity");

            }
        }
    }
}